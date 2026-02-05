using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.IO.Ports
{
	// Token: 0x02000411 RID: 1041
	[MonitoringDescription("SerialPortDesc")]
	public class SerialPort : Component
	{
		// Token: 0x1400003A RID: 58
		// (add) Token: 0x060026C3 RID: 9923 RVA: 0x000B203C File Offset: 0x000B023C
		// (remove) Token: 0x060026C4 RID: 9924 RVA: 0x000B2074 File Offset: 0x000B0274
		[MonitoringDescription("SerialErrorReceived")]
		public event SerialErrorReceivedEventHandler ErrorReceived;

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x060026C5 RID: 9925 RVA: 0x000B20AC File Offset: 0x000B02AC
		// (remove) Token: 0x060026C6 RID: 9926 RVA: 0x000B20E4 File Offset: 0x000B02E4
		[MonitoringDescription("SerialPinChanged")]
		public event SerialPinChangedEventHandler PinChanged;

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x060026C7 RID: 9927 RVA: 0x000B211C File Offset: 0x000B031C
		// (remove) Token: 0x060026C8 RID: 9928 RVA: 0x000B2154 File Offset: 0x000B0354
		[MonitoringDescription("SerialDataReceived")]
		public event SerialDataReceivedEventHandler DataReceived;

		// Token: 0x1700098F RID: 2447
		// (get) Token: 0x060026C9 RID: 9929 RVA: 0x000B2189 File Offset: 0x000B0389
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Stream BaseStream
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("BaseStream_Invalid_Not_Open"));
				}
				return this.internalSerialStream;
			}
		}

		// Token: 0x17000990 RID: 2448
		// (get) Token: 0x060026CA RID: 9930 RVA: 0x000B21A9 File Offset: 0x000B03A9
		// (set) Token: 0x060026CB RID: 9931 RVA: 0x000B21B1 File Offset: 0x000B03B1
		[Browsable(true)]
		[DefaultValue(9600)]
		[MonitoringDescription("BaudRate")]
		public int BaudRate
		{
			get
			{
				return this.baudRate;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("BaudRate", SR.GetString("ArgumentOutOfRange_NeedPosNum"));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.BaudRate = value;
				}
				this.baudRate = value;
			}
		}

		// Token: 0x17000991 RID: 2449
		// (get) Token: 0x060026CC RID: 9932 RVA: 0x000B21E7 File Offset: 0x000B03E7
		// (set) Token: 0x060026CD RID: 9933 RVA: 0x000B220C File Offset: 0x000B040C
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool BreakState
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				return this.internalSerialStream.BreakState;
			}
			set
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				this.internalSerialStream.BreakState = value;
			}
		}

		// Token: 0x17000992 RID: 2450
		// (get) Token: 0x060026CE RID: 9934 RVA: 0x000B2232 File Offset: 0x000B0432
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int BytesToWrite
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				return this.internalSerialStream.BytesToWrite;
			}
		}

		// Token: 0x17000993 RID: 2451
		// (get) Token: 0x060026CF RID: 9935 RVA: 0x000B2257 File Offset: 0x000B0457
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public int BytesToRead
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				return this.internalSerialStream.BytesToRead + this.CachedBytesToRead;
			}
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x060026D0 RID: 9936 RVA: 0x000B2283 File Offset: 0x000B0483
		private int CachedBytesToRead
		{
			get
			{
				return this.readLen - this.readPos;
			}
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x060026D1 RID: 9937 RVA: 0x000B2292 File Offset: 0x000B0492
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CDHolding
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				return this.internalSerialStream.CDHolding;
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x060026D2 RID: 9938 RVA: 0x000B22B7 File Offset: 0x000B04B7
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool CtsHolding
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				return this.internalSerialStream.CtsHolding;
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x060026D3 RID: 9939 RVA: 0x000B22DC File Offset: 0x000B04DC
		// (set) Token: 0x060026D4 RID: 9940 RVA: 0x000B22E4 File Offset: 0x000B04E4
		[Browsable(true)]
		[DefaultValue(8)]
		[MonitoringDescription("DataBits")]
		public int DataBits
		{
			get
			{
				return this.dataBits;
			}
			set
			{
				if (value < 5 || value > 8)
				{
					throw new ArgumentOutOfRangeException("DataBits", SR.GetString("ArgumentOutOfRange_Bounds_Lower_Upper", new object[] { 5, 8 }));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.DataBits = value;
				}
				this.dataBits = value;
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x060026D5 RID: 9941 RVA: 0x000B2341 File Offset: 0x000B0541
		// (set) Token: 0x060026D6 RID: 9942 RVA: 0x000B2349 File Offset: 0x000B0549
		[Browsable(true)]
		[DefaultValue(false)]
		[MonitoringDescription("DiscardNull")]
		public bool DiscardNull
		{
			get
			{
				return this.discardNull;
			}
			set
			{
				if (this.IsOpen)
				{
					this.internalSerialStream.DiscardNull = value;
				}
				this.discardNull = value;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x060026D7 RID: 9943 RVA: 0x000B2366 File Offset: 0x000B0566
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool DsrHolding
		{
			get
			{
				if (!this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Port_not_open"));
				}
				return this.internalSerialStream.DsrHolding;
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x060026D8 RID: 9944 RVA: 0x000B238B File Offset: 0x000B058B
		// (set) Token: 0x060026D9 RID: 9945 RVA: 0x000B23AC File Offset: 0x000B05AC
		[Browsable(true)]
		[DefaultValue(false)]
		[MonitoringDescription("DtrEnable")]
		public bool DtrEnable
		{
			get
			{
				if (this.IsOpen)
				{
					this.dtrEnable = this.internalSerialStream.DtrEnable;
				}
				return this.dtrEnable;
			}
			set
			{
				if (this.IsOpen)
				{
					this.internalSerialStream.DtrEnable = value;
				}
				this.dtrEnable = value;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x060026DA RID: 9946 RVA: 0x000B23C9 File Offset: 0x000B05C9
		// (set) Token: 0x060026DB RID: 9947 RVA: 0x000B23D4 File Offset: 0x000B05D4
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription("Encoding")]
		public Encoding Encoding
		{
			get
			{
				return this.encoding;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Encoding");
				}
				if (!(value is ASCIIEncoding) && !(value is UTF8Encoding) && !(value is UnicodeEncoding) && !(value is UTF32Encoding) && ((value.CodePage >= 50000 && value.CodePage != 54936) || !(value.GetType().Assembly == typeof(string).Assembly)))
				{
					throw new ArgumentException(SR.GetString("NotSupportedEncoding", new object[] { value.WebName }), "value");
				}
				this.encoding = value;
				this.decoder = this.encoding.GetDecoder();
				this.maxByteCountForSingleChar = this.encoding.GetMaxByteCount(1);
				this.singleCharBuffer = null;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x060026DC RID: 9948 RVA: 0x000B249F File Offset: 0x000B069F
		// (set) Token: 0x060026DD RID: 9949 RVA: 0x000B24A7 File Offset: 0x000B06A7
		[Browsable(true)]
		[DefaultValue(Handshake.None)]
		[MonitoringDescription("Handshake")]
		public Handshake Handshake
		{
			get
			{
				return this.handshake;
			}
			set
			{
				if (value < Handshake.None || value > Handshake.RequestToSendXOnXOff)
				{
					throw new ArgumentOutOfRangeException("Handshake", SR.GetString("ArgumentOutOfRange_Enum"));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.Handshake = value;
				}
				this.handshake = value;
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x060026DE RID: 9950 RVA: 0x000B24E1 File Offset: 0x000B06E1
		[Browsable(false)]
		public bool IsOpen
		{
			get
			{
				return this.internalSerialStream != null && this.internalSerialStream.IsOpen;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x060026DF RID: 9951 RVA: 0x000B24F8 File Offset: 0x000B06F8
		// (set) Token: 0x060026E0 RID: 9952 RVA: 0x000B2500 File Offset: 0x000B0700
		[Browsable(false)]
		[DefaultValue("\n")]
		[MonitoringDescription("NewLine")]
		public string NewLine
		{
			get
			{
				return this.newLine;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException();
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[] { "NewLine" }));
				}
				this.newLine = value;
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x060026E1 RID: 9953 RVA: 0x000B2538 File Offset: 0x000B0738
		// (set) Token: 0x060026E2 RID: 9954 RVA: 0x000B2540 File Offset: 0x000B0740
		[Browsable(true)]
		[DefaultValue(Parity.None)]
		[MonitoringDescription("Parity")]
		public Parity Parity
		{
			get
			{
				return this.parity;
			}
			set
			{
				if (value < Parity.None || value > Parity.Space)
				{
					throw new ArgumentOutOfRangeException("Parity", SR.GetString("ArgumentOutOfRange_Enum"));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.Parity = value;
				}
				this.parity = value;
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x060026E3 RID: 9955 RVA: 0x000B257A File Offset: 0x000B077A
		// (set) Token: 0x060026E4 RID: 9956 RVA: 0x000B2582 File Offset: 0x000B0782
		[Browsable(true)]
		[DefaultValue(63)]
		[MonitoringDescription("ParityReplace")]
		public byte ParityReplace
		{
			get
			{
				return this.parityReplace;
			}
			set
			{
				if (this.IsOpen)
				{
					this.internalSerialStream.ParityReplace = value;
				}
				this.parityReplace = value;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x060026E5 RID: 9957 RVA: 0x000B259F File Offset: 0x000B079F
		// (set) Token: 0x060026E6 RID: 9958 RVA: 0x000B25A8 File Offset: 0x000B07A8
		[Browsable(true)]
		[DefaultValue("COM1")]
		[MonitoringDescription("PortName")]
		public string PortName
		{
			get
			{
				return this.portName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("PortName");
				}
				if (value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("PortNameEmpty_String"), "PortName");
				}
				if (value.StartsWith("\\\\", StringComparison.Ordinal))
				{
					throw new ArgumentException(SR.GetString("Arg_SecurityException"), "PortName");
				}
				if (this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Cant_be_set_when_open", new object[] { "PortName" }));
				}
				this.portName = value;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x060026E7 RID: 9959 RVA: 0x000B2630 File Offset: 0x000B0830
		// (set) Token: 0x060026E8 RID: 9960 RVA: 0x000B2638 File Offset: 0x000B0838
		[Browsable(true)]
		[DefaultValue(4096)]
		[MonitoringDescription("ReadBufferSize")]
		public int ReadBufferSize
		{
			get
			{
				return this.readBufferSize;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Cant_be_set_when_open", new object[] { "value" }));
				}
				this.readBufferSize = value;
			}
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x000B2676 File Offset: 0x000B0876
		// (set) Token: 0x060026EA RID: 9962 RVA: 0x000B267E File Offset: 0x000B087E
		[Browsable(true)]
		[DefaultValue(-1)]
		[MonitoringDescription("ReadTimeout")]
		public int ReadTimeout
		{
			get
			{
				return this.readTimeout;
			}
			set
			{
				if (value < 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("ReadTimeout", SR.GetString("ArgumentOutOfRange_Timeout"));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.ReadTimeout = value;
				}
				this.readTimeout = value;
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x060026EB RID: 9963 RVA: 0x000B26B8 File Offset: 0x000B08B8
		// (set) Token: 0x060026EC RID: 9964 RVA: 0x000B26C0 File Offset: 0x000B08C0
		[Browsable(true)]
		[DefaultValue(1)]
		[MonitoringDescription("ReceivedBytesThreshold")]
		public int ReceivedBytesThreshold
		{
			get
			{
				return this.receivedBytesThreshold;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("ReceivedBytesThreshold", SR.GetString("ArgumentOutOfRange_NeedPosNum"));
				}
				this.receivedBytesThreshold = value;
				if (this.IsOpen)
				{
					SerialDataReceivedEventArgs serialDataReceivedEventArgs = new SerialDataReceivedEventArgs(SerialData.Chars);
					this.CatchReceivedEvents(this, serialDataReceivedEventArgs);
				}
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x000B2704 File Offset: 0x000B0904
		// (set) Token: 0x060026EE RID: 9966 RVA: 0x000B2725 File Offset: 0x000B0925
		[Browsable(true)]
		[DefaultValue(false)]
		[MonitoringDescription("RtsEnable")]
		public bool RtsEnable
		{
			get
			{
				if (this.IsOpen)
				{
					this.rtsEnable = this.internalSerialStream.RtsEnable;
				}
				return this.rtsEnable;
			}
			set
			{
				if (this.IsOpen)
				{
					this.internalSerialStream.RtsEnable = value;
				}
				this.rtsEnable = value;
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x000B2742 File Offset: 0x000B0942
		// (set) Token: 0x060026F0 RID: 9968 RVA: 0x000B274A File Offset: 0x000B094A
		[Browsable(true)]
		[DefaultValue(StopBits.One)]
		[MonitoringDescription("StopBits")]
		public StopBits StopBits
		{
			get
			{
				return this.stopBits;
			}
			set
			{
				if (value < StopBits.One || value > StopBits.OnePointFive)
				{
					throw new ArgumentOutOfRangeException("StopBits", SR.GetString("ArgumentOutOfRange_Enum"));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.StopBits = value;
				}
				this.stopBits = value;
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x060026F1 RID: 9969 RVA: 0x000B2784 File Offset: 0x000B0984
		// (set) Token: 0x060026F2 RID: 9970 RVA: 0x000B278C File Offset: 0x000B098C
		[Browsable(true)]
		[DefaultValue(2048)]
		[MonitoringDescription("WriteBufferSize")]
		public int WriteBufferSize
		{
			get
			{
				return this.writeBufferSize;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (this.IsOpen)
				{
					throw new InvalidOperationException(SR.GetString("Cant_be_set_when_open", new object[] { "value" }));
				}
				this.writeBufferSize = value;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x060026F3 RID: 9971 RVA: 0x000B27CA File Offset: 0x000B09CA
		// (set) Token: 0x060026F4 RID: 9972 RVA: 0x000B27D2 File Offset: 0x000B09D2
		[Browsable(true)]
		[DefaultValue(-1)]
		[MonitoringDescription("WriteTimeout")]
		public int WriteTimeout
		{
			get
			{
				return this.writeTimeout;
			}
			set
			{
				if (value <= 0 && value != -1)
				{
					throw new ArgumentOutOfRangeException("WriteTimeout", SR.GetString("ArgumentOutOfRange_WriteTimeout"));
				}
				if (this.IsOpen)
				{
					this.internalSerialStream.WriteTimeout = value;
				}
				this.writeTimeout = value;
			}
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x000B280C File Offset: 0x000B0A0C
		public SerialPort(IContainer container)
		{
			container.Add(this);
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x000B28D0 File Offset: 0x000B0AD0
		public SerialPort()
		{
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x000B298D File Offset: 0x000B0B8D
		public SerialPort(string portName)
			: this(portName, 9600, Parity.None, 8, StopBits.One)
		{
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x000B299E File Offset: 0x000B0B9E
		public SerialPort(string portName, int baudRate)
			: this(portName, baudRate, Parity.None, 8, StopBits.One)
		{
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x000B29AB File Offset: 0x000B0BAB
		public SerialPort(string portName, int baudRate, Parity parity)
			: this(portName, baudRate, parity, 8, StopBits.One)
		{
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x000B29B8 File Offset: 0x000B0BB8
		public SerialPort(string portName, int baudRate, Parity parity, int dataBits)
			: this(portName, baudRate, parity, dataBits, StopBits.One)
		{
		}

		// Token: 0x060026FB RID: 9979 RVA: 0x000B29C8 File Offset: 0x000B0BC8
		public SerialPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
		{
			this.PortName = portName;
			this.BaudRate = baudRate;
			this.Parity = parity;
			this.DataBits = dataBits;
			this.StopBits = stopBits;
		}

		// Token: 0x060026FC RID: 9980 RVA: 0x000B2AAA File Offset: 0x000B0CAA
		public void Close()
		{
			base.Dispose();
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x000B2AB2 File Offset: 0x000B0CB2
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.IsOpen)
			{
				this.internalSerialStream.Flush();
				this.internalSerialStream.Close();
				this.internalSerialStream = null;
			}
			base.Dispose(disposing);
		}

		// Token: 0x060026FE RID: 9982 RVA: 0x000B2AE4 File Offset: 0x000B0CE4
		public void DiscardInBuffer()
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			this.internalSerialStream.DiscardInBuffer();
			this.readPos = (this.readLen = 0);
		}

		// Token: 0x060026FF RID: 9983 RVA: 0x000B2B24 File Offset: 0x000B0D24
		public void DiscardOutBuffer()
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			this.internalSerialStream.DiscardOutBuffer();
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x000B2B4C File Offset: 0x000B0D4C
		public static string[] GetPortNames()
		{
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			string[] array = null;
			RegistryPermission registryPermission = new RegistryPermission(RegistryPermissionAccess.Read, "HKEY_LOCAL_MACHINE\\HARDWARE\\DEVICEMAP\\SERIALCOMM");
			registryPermission.Assert();
			try
			{
				registryKey = Registry.LocalMachine;
				registryKey2 = registryKey.OpenSubKey("HARDWARE\\DEVICEMAP\\SERIALCOMM", false);
				if (registryKey2 != null)
				{
					string[] valueNames = registryKey2.GetValueNames();
					array = new string[valueNames.Length];
					for (int i = 0; i < valueNames.Length; i++)
					{
						array[i] = (string)registryKey2.GetValue(valueNames[i]);
					}
				}
			}
			finally
			{
				if (registryKey != null)
				{
					registryKey.Close();
				}
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
				CodeAccessPermission.RevertAssert();
			}
			if (array == null)
			{
				array = new string[0];
			}
			return array;
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x000B2BF8 File Offset: 0x000B0DF8
		public void Open()
		{
			if (this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_already_open"));
			}
			new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).Demand();
			this.internalSerialStream = new SerialStream(this.portName, this.baudRate, this.parity, this.dataBits, this.stopBits, this.readTimeout, this.writeTimeout, this.handshake, this.dtrEnable, this.rtsEnable, this.discardNull, this.parityReplace);
			this.internalSerialStream.SetBufferSizes(this.readBufferSize, this.writeBufferSize);
			this.internalSerialStream.ErrorReceived += this.CatchErrorEvents;
			this.internalSerialStream.PinChanged += this.CatchPinChangedEvents;
			this.internalSerialStream.DataReceived += this.CatchReceivedEvents;
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x000B2CD8 File Offset: 0x000B0ED8
		public int Read(byte[] buffer, int offset, int count)
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", SR.GetString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			int num = 0;
			if (this.CachedBytesToRead >= 1)
			{
				num = Math.Min(this.CachedBytesToRead, count);
				Buffer.BlockCopy(this.inBuffer, this.readPos, buffer, offset, num);
				this.readPos += num;
				if (num == count)
				{
					if (this.readPos == this.readLen)
					{
						this.readPos = (this.readLen = 0);
					}
					return count;
				}
				if (this.BytesToRead == 0)
				{
					return num;
				}
			}
			this.readLen = (this.readPos = 0);
			int num2 = count - num;
			num += this.internalSerialStream.Read(buffer, offset + num, num2);
			this.decoder.Reset();
			return num;
		}

		// Token: 0x06002703 RID: 9987 RVA: 0x000B2DFA File Offset: 0x000B0FFA
		public int ReadChar()
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			return this.ReadOneChar(this.readTimeout);
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x000B2E20 File Offset: 0x000B1020
		private int ReadOneChar(int timeout)
		{
			int num = 0;
			if (this.decoder.GetCharCount(this.inBuffer, this.readPos, this.CachedBytesToRead) != 0)
			{
				int num2 = this.readPos;
				do
				{
					this.readPos++;
				}
				while (this.decoder.GetCharCount(this.inBuffer, num2, this.readPos - num2) < 1);
				try
				{
					this.decoder.GetChars(this.inBuffer, num2, this.readPos - num2, this.oneChar, 0);
				}
				catch
				{
					this.readPos = num2;
					throw;
				}
				return (int)this.oneChar[0];
			}
			if (timeout != 0)
			{
				int tickCount = Environment.TickCount;
				for (;;)
				{
					int num3;
					if (timeout == -1)
					{
						num3 = this.internalSerialStream.ReadByte(-1);
					}
					else
					{
						if (timeout - num < 0)
						{
							break;
						}
						num3 = this.internalSerialStream.ReadByte(timeout - num);
						num = Environment.TickCount - tickCount;
					}
					this.MaybeResizeBuffer(1);
					byte[] array = this.inBuffer;
					int num4 = this.readLen;
					this.readLen = num4 + 1;
					array[num4] = (byte)num3;
					if (this.decoder.GetCharCount(this.inBuffer, this.readPos, this.readLen - this.readPos) >= 1)
					{
						goto Block_8;
					}
				}
				throw new TimeoutException();
				Block_8:
				this.decoder.GetChars(this.inBuffer, this.readPos, this.readLen - this.readPos, this.oneChar, 0);
				this.readLen = (this.readPos = 0);
				return (int)this.oneChar[0];
			}
			int num5 = this.internalSerialStream.BytesToRead;
			if (num5 == 0)
			{
				num5 = 1;
			}
			this.MaybeResizeBuffer(num5);
			this.readLen += this.internalSerialStream.Read(this.inBuffer, this.readLen, num5);
			if (this.ReadBufferIntoChars(this.oneChar, 0, 1, false) == 0)
			{
				throw new TimeoutException();
			}
			return (int)this.oneChar[0];
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000B3000 File Offset: 0x000B1200
		public int Read(char[] buffer, int offset, int count)
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", SR.GetString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			return this.InternalRead(buffer, offset, count, this.readTimeout, false);
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000B3098 File Offset: 0x000B1298
		private int InternalRead(char[] buffer, int offset, int count, int timeout, bool countMultiByteCharsAsOne)
		{
			if (count == 0)
			{
				return 0;
			}
			int tickCount = Environment.TickCount;
			int bytesToRead = this.internalSerialStream.BytesToRead;
			this.MaybeResizeBuffer(bytesToRead);
			this.readLen += this.internalSerialStream.Read(this.inBuffer, this.readLen, bytesToRead);
			int charCount = this.decoder.GetCharCount(this.inBuffer, this.readPos, this.CachedBytesToRead);
			if (charCount > 0)
			{
				return this.ReadBufferIntoChars(buffer, offset, count, countMultiByteCharsAsOne);
			}
			if (timeout == 0)
			{
				throw new TimeoutException();
			}
			int maxByteCount = this.Encoding.GetMaxByteCount(count);
			int num;
			for (;;)
			{
				this.MaybeResizeBuffer(maxByteCount);
				this.readLen += this.internalSerialStream.Read(this.inBuffer, this.readLen, maxByteCount);
				num = this.ReadBufferIntoChars(buffer, offset, count, countMultiByteCharsAsOne);
				if (num > 0)
				{
					break;
				}
				if (timeout != -1 && timeout - SerialPort.GetElapsedTime(Environment.TickCount, tickCount) <= 0)
				{
					goto Block_6;
				}
			}
			return num;
			Block_6:
			throw new TimeoutException();
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000B318C File Offset: 0x000B138C
		private int ReadBufferIntoChars(char[] buffer, int offset, int count, bool countMultiByteCharsAsOne)
		{
			int num = Math.Min(count, this.CachedBytesToRead);
			DecoderReplacementFallback decoderReplacementFallback = this.encoding.DecoderFallback as DecoderReplacementFallback;
			if (this.encoding.IsSingleByte && this.encoding.GetMaxCharCount(num) == num && decoderReplacementFallback != null && decoderReplacementFallback.MaxCharCount == 1)
			{
				this.decoder.GetChars(this.inBuffer, this.readPos, num, buffer, offset);
				this.readPos += num;
				if (this.readPos == this.readLen)
				{
					this.readPos = (this.readLen = 0);
				}
				return num;
			}
			int num2 = 0;
			int num3 = 0;
			int num4 = this.readPos;
			do
			{
				int num5 = Math.Min(count - num3, this.readLen - this.readPos - num2);
				if (num5 <= 0)
				{
					break;
				}
				num2 += num5;
				num5 = this.readPos + num2 - num4;
				int charCount = this.decoder.GetCharCount(this.inBuffer, num4, num5);
				if (charCount > 0)
				{
					if (num3 + charCount > count && !countMultiByteCharsAsOne)
					{
						break;
					}
					int num6 = num5;
					do
					{
						num6--;
					}
					while (this.decoder.GetCharCount(this.inBuffer, num4, num6) == charCount);
					this.decoder.GetChars(this.inBuffer, num4, num6 + 1, buffer, offset + num3);
					num4 = num4 + num6 + 1;
				}
				num3 += charCount;
			}
			while (num3 < count && num2 < this.CachedBytesToRead);
			this.readPos = num4;
			if (this.readPos == this.readLen)
			{
				this.readPos = (this.readLen = 0);
			}
			return num3;
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000B331C File Offset: 0x000B151C
		public int ReadByte()
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (this.readLen != this.readPos)
			{
				byte[] array = this.inBuffer;
				int num = this.readPos;
				this.readPos = num + 1;
				return array[num];
			}
			this.decoder.Reset();
			return this.internalSerialStream.ReadByte();
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000B3380 File Offset: 0x000B1580
		public string ReadExisting()
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			byte[] array = new byte[this.BytesToRead];
			if (this.readPos < this.readLen)
			{
				Buffer.BlockCopy(this.inBuffer, this.readPos, array, 0, this.CachedBytesToRead);
			}
			this.internalSerialStream.Read(array, this.CachedBytesToRead, array.Length - this.CachedBytesToRead);
			Decoder decoder = this.Encoding.GetDecoder();
			int charCount = decoder.GetCharCount(array, 0, array.Length);
			int num = array.Length;
			if (charCount == 0)
			{
				Buffer.BlockCopy(array, 0, this.inBuffer, 0, array.Length);
				this.readPos = 0;
				this.readLen = array.Length;
				return "";
			}
			do
			{
				decoder.Reset();
				num--;
			}
			while (decoder.GetCharCount(array, 0, num) == charCount);
			this.readPos = 0;
			this.readLen = array.Length - (num + 1);
			Buffer.BlockCopy(array, num + 1, this.inBuffer, 0, array.Length - (num + 1));
			return this.Encoding.GetString(array, 0, num + 1);
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000B348C File Offset: 0x000B168C
		public string ReadLine()
		{
			return this.ReadTo(this.NewLine);
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x000B349C File Offset: 0x000B169C
		public string ReadTo(string value)
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length == 0)
			{
				throw new ArgumentException(SR.GetString("InvalidNullEmptyArgument", new object[] { "value" }));
			}
			int tickCount = Environment.TickCount;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder();
			char c = value[value.Length - 1];
			int bytesToRead = this.internalSerialStream.BytesToRead;
			this.MaybeResizeBuffer(bytesToRead);
			this.readLen += this.internalSerialStream.Read(this.inBuffer, this.readLen, bytesToRead);
			int num2 = this.readPos;
			if (this.singleCharBuffer == null)
			{
				this.singleCharBuffer = new char[this.maxByteCountForSingleChar];
			}
			string text2;
			try
			{
				for (;;)
				{
					int num3;
					if (this.readTimeout == -1)
					{
						num3 = this.InternalRead(this.singleCharBuffer, 0, 1, this.readTimeout, true);
					}
					else
					{
						if (this.readTimeout - num < 0)
						{
							break;
						}
						int tickCount2 = Environment.TickCount;
						num3 = this.InternalRead(this.singleCharBuffer, 0, 1, this.readTimeout - num, true);
						num += Environment.TickCount - tickCount2;
					}
					stringBuilder.Append(this.singleCharBuffer, 0, num3);
					if (c == this.singleCharBuffer[num3 - 1] && stringBuilder.Length >= value.Length)
					{
						bool flag = true;
						for (int i = 2; i <= value.Length; i++)
						{
							if (value[value.Length - i] != stringBuilder[stringBuilder.Length - i])
							{
								flag = false;
								break;
							}
						}
						if (flag)
						{
							goto Block_11;
						}
					}
				}
				throw new TimeoutException();
				Block_11:
				string text = stringBuilder.ToString(0, stringBuilder.Length - value.Length);
				if (this.readPos == this.readLen)
				{
					this.readPos = (this.readLen = 0);
				}
				text2 = text;
			}
			catch
			{
				byte[] bytes = this.encoding.GetBytes(stringBuilder.ToString());
				if (bytes.Length != 0)
				{
					int cachedBytesToRead = this.CachedBytesToRead;
					byte[] array = new byte[cachedBytesToRead];
					if (cachedBytesToRead > 0)
					{
						Buffer.BlockCopy(this.inBuffer, this.readPos, array, 0, cachedBytesToRead);
					}
					this.readPos = 0;
					this.readLen = 0;
					this.MaybeResizeBuffer(bytes.Length + cachedBytesToRead);
					Buffer.BlockCopy(bytes, 0, this.inBuffer, this.readLen, bytes.Length);
					this.readLen += bytes.Length;
					if (cachedBytesToRead > 0)
					{
						Buffer.BlockCopy(array, 0, this.inBuffer, this.readLen, cachedBytesToRead);
						this.readLen += cachedBytesToRead;
					}
				}
				throw;
			}
			return text2;
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000B375C File Offset: 0x000B195C
		public void Write(string text)
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (text == null)
			{
				throw new ArgumentNullException("text");
			}
			if (text.Length == 0)
			{
				return;
			}
			byte[] bytes = this.encoding.GetBytes(text);
			this.internalSerialStream.Write(bytes, 0, bytes.Length, this.writeTimeout);
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x000B37BC File Offset: 0x000B19BC
		public void Write(char[] buffer, int offset, int count)
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			if (buffer.Length == 0)
			{
				return;
			}
			byte[] bytes = this.Encoding.GetBytes(buffer, offset, count);
			this.Write(bytes, 0, bytes.Length);
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x000B3858 File Offset: 0x000B1A58
		public void Write(byte[] buffer, int offset, int count)
		{
			if (!this.IsOpen)
			{
				throw new InvalidOperationException(SR.GetString("Port_not_open"));
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", SR.GetString("ArgumentNull_Buffer"));
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count", SR.GetString("ArgumentOutOfRange_NeedNonNegNumRequired"));
			}
			if (buffer.Length - offset < count)
			{
				throw new ArgumentException(SR.GetString("Argument_InvalidOffLen"));
			}
			if (buffer.Length == 0)
			{
				return;
			}
			this.internalSerialStream.Write(buffer, offset, count, this.writeTimeout);
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x000B38F8 File Offset: 0x000B1AF8
		public void WriteLine(string text)
		{
			this.Write(text + this.NewLine);
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x000B390C File Offset: 0x000B1B0C
		private void CatchErrorEvents(object src, SerialErrorReceivedEventArgs e)
		{
			SerialErrorReceivedEventHandler errorReceived = this.ErrorReceived;
			SerialStream serialStream = this.internalSerialStream;
			if (errorReceived != null && serialStream != null)
			{
				SerialStream serialStream2 = serialStream;
				lock (serialStream2)
				{
					if (serialStream.IsOpen)
					{
						errorReceived(this, e);
					}
				}
			}
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x000B3968 File Offset: 0x000B1B68
		private void CatchPinChangedEvents(object src, SerialPinChangedEventArgs e)
		{
			SerialPinChangedEventHandler pinChanged = this.PinChanged;
			SerialStream serialStream = this.internalSerialStream;
			if (pinChanged != null && serialStream != null)
			{
				SerialStream serialStream2 = serialStream;
				lock (serialStream2)
				{
					if (serialStream.IsOpen)
					{
						pinChanged(this, e);
					}
				}
			}
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x000B39C4 File Offset: 0x000B1BC4
		private void CatchReceivedEvents(object src, SerialDataReceivedEventArgs e)
		{
			SerialDataReceivedEventHandler dataReceived = this.DataReceived;
			SerialStream serialStream = this.internalSerialStream;
			if (dataReceived != null && serialStream != null)
			{
				SerialStream serialStream2 = serialStream;
				lock (serialStream2)
				{
					bool flag2 = false;
					try
					{
						flag2 = serialStream.IsOpen && (SerialData.Eof == e.EventType || this.BytesToRead >= this.receivedBytesThreshold);
					}
					catch
					{
					}
					finally
					{
						if (flag2)
						{
							dataReceived(this, e);
						}
					}
				}
			}
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000B3A64 File Offset: 0x000B1C64
		private void CompactBuffer()
		{
			Buffer.BlockCopy(this.inBuffer, this.readPos, this.inBuffer, 0, this.CachedBytesToRead);
			this.readLen = this.CachedBytesToRead;
			this.readPos = 0;
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x000B3A98 File Offset: 0x000B1C98
		private void MaybeResizeBuffer(int additionalByteLength)
		{
			if (additionalByteLength + this.readLen <= this.inBuffer.Length)
			{
				return;
			}
			if (this.CachedBytesToRead + additionalByteLength <= this.inBuffer.Length / 2)
			{
				this.CompactBuffer();
				return;
			}
			int num = Math.Max(this.CachedBytesToRead + additionalByteLength, this.inBuffer.Length * 2);
			byte[] array = new byte[num];
			Buffer.BlockCopy(this.inBuffer, this.readPos, array, 0, this.CachedBytesToRead);
			this.readLen = this.CachedBytesToRead;
			this.readPos = 0;
			this.inBuffer = array;
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x000B3B28 File Offset: 0x000B1D28
		private static int GetElapsedTime(int currentTickCount, int startTickCount)
		{
			int num = currentTickCount - startTickCount;
			if (num < 0)
			{
				return int.MaxValue;
			}
			return num;
		}

		// Token: 0x04002101 RID: 8449
		public const int InfiniteTimeout = -1;

		// Token: 0x04002102 RID: 8450
		private const int defaultDataBits = 8;

		// Token: 0x04002103 RID: 8451
		private const Parity defaultParity = Parity.None;

		// Token: 0x04002104 RID: 8452
		private const StopBits defaultStopBits = StopBits.One;

		// Token: 0x04002105 RID: 8453
		private const Handshake defaultHandshake = Handshake.None;

		// Token: 0x04002106 RID: 8454
		private const int defaultBufferSize = 1024;

		// Token: 0x04002107 RID: 8455
		private const string defaultPortName = "COM1";

		// Token: 0x04002108 RID: 8456
		private const int defaultBaudRate = 9600;

		// Token: 0x04002109 RID: 8457
		private const bool defaultDtrEnable = false;

		// Token: 0x0400210A RID: 8458
		private const bool defaultRtsEnable = false;

		// Token: 0x0400210B RID: 8459
		private const bool defaultDiscardNull = false;

		// Token: 0x0400210C RID: 8460
		private const byte defaultParityReplace = 63;

		// Token: 0x0400210D RID: 8461
		private const int defaultReceivedBytesThreshold = 1;

		// Token: 0x0400210E RID: 8462
		private const int defaultReadTimeout = -1;

		// Token: 0x0400210F RID: 8463
		private const int defaultWriteTimeout = -1;

		// Token: 0x04002110 RID: 8464
		private const int defaultReadBufferSize = 4096;

		// Token: 0x04002111 RID: 8465
		private const int defaultWriteBufferSize = 2048;

		// Token: 0x04002112 RID: 8466
		private const int maxDataBits = 8;

		// Token: 0x04002113 RID: 8467
		private const int minDataBits = 5;

		// Token: 0x04002114 RID: 8468
		private const string defaultNewLine = "\n";

		// Token: 0x04002115 RID: 8469
		private const string SERIAL_NAME = "\\Device\\Serial";

		// Token: 0x04002116 RID: 8470
		private int baudRate = 9600;

		// Token: 0x04002117 RID: 8471
		private int dataBits = 8;

		// Token: 0x04002118 RID: 8472
		private Parity parity;

		// Token: 0x04002119 RID: 8473
		private StopBits stopBits = StopBits.One;

		// Token: 0x0400211A RID: 8474
		private string portName = "COM1";

		// Token: 0x0400211B RID: 8475
		private Encoding encoding = Encoding.ASCII;

		// Token: 0x0400211C RID: 8476
		private Decoder decoder = Encoding.ASCII.GetDecoder();

		// Token: 0x0400211D RID: 8477
		private int maxByteCountForSingleChar = Encoding.ASCII.GetMaxByteCount(1);

		// Token: 0x0400211E RID: 8478
		private Handshake handshake;

		// Token: 0x0400211F RID: 8479
		private int readTimeout = -1;

		// Token: 0x04002120 RID: 8480
		private int writeTimeout = -1;

		// Token: 0x04002121 RID: 8481
		private int receivedBytesThreshold = 1;

		// Token: 0x04002122 RID: 8482
		private bool discardNull;

		// Token: 0x04002123 RID: 8483
		private bool dtrEnable;

		// Token: 0x04002124 RID: 8484
		private bool rtsEnable;

		// Token: 0x04002125 RID: 8485
		private byte parityReplace = 63;

		// Token: 0x04002126 RID: 8486
		private string newLine = "\n";

		// Token: 0x04002127 RID: 8487
		private int readBufferSize = 4096;

		// Token: 0x04002128 RID: 8488
		private int writeBufferSize = 2048;

		// Token: 0x04002129 RID: 8489
		private SerialStream internalSerialStream;

		// Token: 0x0400212A RID: 8490
		private byte[] inBuffer = new byte[1024];

		// Token: 0x0400212B RID: 8491
		private int readPos;

		// Token: 0x0400212C RID: 8492
		private int readLen;

		// Token: 0x0400212D RID: 8493
		private char[] oneChar = new char[1];

		// Token: 0x0400212E RID: 8494
		private char[] singleCharBuffer;
	}
}
