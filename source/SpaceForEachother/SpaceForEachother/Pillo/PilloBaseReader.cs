using System;
using System.Collections;
using System.IO.Ports;
using System.Threading;
using Microsoft.Xna.Framework;
using System.Diagnostics;

public delegate void PilloConnectionLostEvent();
public delegate void PilloReConnectedEvent();
public delegate void PilloDisconnectedEvent();
public delegate void PilloConnectedEvent();

public abstract class PilloBaseReader
{
	public enum PilloConnectionState
	{
		PCS_CONNECT,
		PCS_DISCONNECT,
		PCS_DISCONNECTED,
		PCS_CONNECTED,
		PCS_SHOULDCONNECT
	}
	//VARS============================================================================================================
	private SerialPort m_SerialPort = null;
	protected Vector2 m_RawValue = Vector2.Zero;
	protected string m_PortName = PilloConfig.UNKNOWN_PORTNAME;
	protected PilloConnectionState m_State = PilloConnectionState.PCS_DISCONNECTED;
	private Thread m_Updater;

	//PROPS===========================================================================================================
	public Vector2 RawValue { get { return m_RawValue; } }
	public string PortName { get { return m_PortName; } }

	public PilloConnectionLostEvent PilloConnectionLost { get; set; }
	public PilloReConnectedEvent PilloReConnected { get; set; }
	public PilloDisconnectedEvent PilloDisconnected { get; set; }
	public PilloConnectedEvent PilloConnected { get; set; }

	//CONST===========================================================================================================
	public PilloBaseReader()
	{
		PilloReConnected += new PilloReConnectedEvent(receivePilloReConnection);
		PilloConnectionLost += new PilloConnectionLostEvent(receivePilloConnectionLost);
		PilloDisconnected += new PilloDisconnectedEvent(receivePilloDisconnected);
		PilloConnected += new PilloConnectedEvent(receivePilloConnected);
		m_Updater = new Thread(updateThread); 
	}

	//FUNC============================================================================================================
	private void updateThread()
	{
		while (true)
		{
			switch (m_State)
			{
				case PilloConnectionState.PCS_CONNECT:
				case PilloConnectionState.PCS_SHOULDCONNECT:
						tryToConnect(); 
						if (isConnected())
						{
							if (m_State == PilloConnectionState.PCS_CONNECT)
							{
								m_State = PilloConnectionState.PCS_CONNECTED;
								invokeConnected();
							}
							else
							{
								m_State = PilloConnectionState.PCS_CONNECTED;
								invokeReConnected();
							}
						}
						else
						{
							Thread.Sleep(500);
						}
					break;
				case PilloConnectionState.PCS_DISCONNECT:
						if (isConnected())
						{
							try
							{
								m_SerialPort.Close();
							}
							catch (Exception ex)
							{
								log("Error closing port");
								log(ex.Message);
							}
						}
						if (m_State != PilloConnectionState.PCS_DISCONNECTED)
						{
							m_SerialPort = null;
							m_State = PilloConnectionState.PCS_DISCONNECTED;
							m_PortName = PilloConfig.UNKNOWN_PORTNAME;
							invokeDisconnected();
						} return;
					break;
				case PilloConnectionState.PCS_CONNECTED:
						if (isConnected())
						{
							try
							{
								string serialValue = m_SerialPort.ReadLine();
								string[] posValues = serialValue.Split(PilloConfig.PARSE_SPLITCHAR);
								m_RawValue = new Vector2(int.Parse(posValues[0]), int.Parse(posValues[1]));
								valuesUpdated();
							}
							catch (System.IO.IOException)
							{
								m_State = PilloConnectionState.PCS_SHOULDCONNECT;
								m_SerialPort = null;
								invokeConnectionLost();
							}
							catch (Exception ex)
							{
								log(ex.Message);
							}
						}
					break;
			}
		}
	}

	public void DisconnectFromPillo()
	{
		m_State = PilloConnectionState.PCS_DISCONNECT;
		if (m_Updater.IsAlive)
		{
			m_Updater.Abort();
		}
	}

	public void ConnectToPillo()
	{
		if (!m_Updater.IsAlive)
		{
			m_Updater.Start();
		}
		m_State = PilloConnectionState.PCS_CONNECT;
	}

	//ABSTRACT========================================================================================================
	protected abstract void valuesUpdated();

	//PRIVATE=========================================================================================================
	private void tryToConnect()
	{
		SerialPort port = null;
		foreach (string portname in SerialPort.GetPortNames())
		{
			port = new SerialPort();
			try
			{
				port.BaudRate = PilloConfig.BAUD_RATE;
				port.ReadTimeout = PilloConfig.READ_TIMEOUT;
				port.NewLine = PilloConfig.PARSE_NEWLINE;
				port.PortName = portname;
				if (!port.IsOpen)
				{
					port.Open();
					string received = port.ReadLine();
					if (!(received.Length == PilloConfig.INPUT_STRING_LENGTH)
						|| !(received.Contains(PilloConfig.PARSE_SPLITCHAR.ToString())))
					{
						port.Close();
						port = null;
					}
					else
					{
						m_SerialPort = port;
						m_PortName = m_SerialPort.PortName;
						break;
					}
				}
			}
			catch
			{
				port.Close();
				port = null;
			}
		}
	}
	private bool isConnected() { return (m_SerialPort != null && m_SerialPort.IsOpen); }
	protected void log(string msg) { if (PilloConfig.USE_DEBUG) { Debug.WriteLine(m_PortName + ": " + msg); } }

	//EVENTS==========================================================================================================
	private void invokeConnected()              { if (PilloConnected != null)       { PilloConnected.Invoke(); } }
	private void invokeDisconnected()           { if (PilloDisconnected != null)    { PilloDisconnected.Invoke(); } }
	private void invokeReConnected()            { if (PilloReConnected != null)     { PilloReConnected.Invoke(); } }
	private void invokeConnectionLost()         { if (PilloConnectionLost != null)  { PilloConnectionLost.Invoke(); } }
	private void receivePilloConnectionLost()   { log("Connection lost"); }
	private void receivePilloReConnection()     { log("ReConnected"); }
	private void receivePilloDisconnected()     { log("Disconnected"); }
	private void receivePilloConnected()        { log("Connected"); }
}

