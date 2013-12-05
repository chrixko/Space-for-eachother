using System;
using System.Collections;
using System.IO.Ports;
using Microsoft.Xna.Framework;

public class PilloReader : PilloBaseReader
{
	//VARS============================================================================================================
	protected Vector2 m_CappedValue = Vector2.Zero;
	protected Vector2 m_Percentage = Vector2.Zero;
	protected Vector2 m_FilteredPercentage = Vector2.Zero;
	protected Vector2 m_Acceleration = Vector2.Zero;
	protected Vector2 m_FilteredAcceleration = Vector2.Zero;

	protected Vector2[] m_PercentageFilterPasses;
	protected Vector2[] m_AccelerationFilterPasses;
	protected int m_CurrPassesIdx = 0;
	protected int m_FilterPassesCount = 5;

	//PROPS===========================================================================================================
	public Vector2 Percentage { get { return m_Percentage; } }
	public Vector2 FilteredPercentage { get { return m_FilteredPercentage; } }
	public Vector2 CappedValue { get { return m_CappedValue; } }
	public Vector2 Acceleration { get { return m_Acceleration; } }
	public Vector2 FilteredAcceleration { get { return m_FilteredAcceleration; } }

	//CONST===========================================================================================================
	public PilloReader()
	{
		m_PercentageFilterPasses = new Vector2[m_FilterPassesCount];
		m_AccelerationFilterPasses = new Vector2[m_FilterPassesCount];
	}

	//FUNC============================================================================================================
	protected override void valuesUpdated()
	{
		Vector2 tmpFilteredPerc = Vector2.Zero;
		Vector2 tmpFilteredAcc = Vector2.Zero;
		Vector2 tmpPrevPercentage = m_Percentage;

		m_CappedValue = new Vector2(MathHelper.Max(PilloConfig.CAP_BOT, MathHelper.Min(this.m_RawValue.X, PilloConfig.CAP_TOP))
								, MathHelper.Max(PilloConfig.CAP_BOT, MathHelper.Min(this.m_RawValue.Y, PilloConfig.CAP_TOP)));
		m_Percentage = new Vector2( (1 - ((m_CappedValue.X - PilloConfig.CAP_BOT) / PilloConfig.CAP_RANGE))
									, (1 - ((m_CappedValue.Y - PilloConfig.CAP_BOT) / PilloConfig.CAP_RANGE)) );
		m_Acceleration = m_Percentage-tmpPrevPercentage;

		m_CurrPassesIdx = (m_CurrPassesIdx + 1) % m_FilterPassesCount;

		m_PercentageFilterPasses[m_CurrPassesIdx] = m_Percentage;
		m_AccelerationFilterPasses[m_CurrPassesIdx] = m_Acceleration;

		int devider = 0;
		for (int i = 0; i < m_FilterPassesCount; i++)
		{
			//normal mean
			tmpFilteredPerc += m_PercentageFilterPasses[i];
			//weighted mean
			int weight = 1;
			if (m_AccelerationFilterPasses[i] != Vector2.Zero)
			{
				weight = 3;
			}
			tmpFilteredAcc += m_AccelerationFilterPasses[i] * weight;
			devider += weight;
		}
		m_FilteredPercentage = tmpFilteredPerc / (float)m_FilterPassesCount;
		m_FilteredAcceleration = tmpFilteredAcc / (float)devider;
	}

	public void ResetFilters()
	{
		try
		{
			m_CurrPassesIdx = 0;
			m_PercentageFilterPasses = new Vector2[m_FilterPassesCount];
			m_AccelerationFilterPasses = new Vector2[m_FilterPassesCount];
		}
		catch (Exception ex)
		{
			log(ex.Message);
		}
	}

	public void ChangeFilterPasses(int passes)
	{
		try
		{
			m_FilterPassesCount = passes;
			ResetFilters();
		}
		catch (Exception ex)
		{
			log(ex.Message);
		}
	}
	//PRIVATE=========================================================================================================
}

