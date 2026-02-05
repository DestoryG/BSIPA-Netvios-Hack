using System;
using System.Runtime.InteropServices;

namespace IPA.Utilities
{
	/// <summary>
	/// Defines helpers for working with Win32 APIs
	/// </summary>
	// Token: 0x02000020 RID: 32
	internal static class Win32
	{
		// Token: 0x0600009F RID: 159
		[DllImport("kernel32.dll")]
		public static extern bool SetConsoleCtrlHandler(Win32.ConsoleCtrlDelegate HandlerRoutine, bool Add);

		/// <summary>
		/// Delegate type to be used as the Handler Routine for SCCH
		/// </summary>
		// Token: 0x020000C9 RID: 201
		// (Invoke) Token: 0x060004B1 RID: 1201
		public delegate bool ConsoleCtrlDelegate(Win32.CtrlTypes CtrlType);

		/// <summary>
		/// Enumerated type for the control messages sent to the handler routine
		/// </summary>
		// Token: 0x020000CA RID: 202
		public enum CtrlTypes : uint
		{
			/// <summary>
			/// The user pressed Ctrl+C
			/// </summary>
			// Token: 0x040001B8 RID: 440
			CTRL_C_EVENT,
			/// <summary>
			/// The user pressed Ctrl+Break
			/// </summary>
			// Token: 0x040001B9 RID: 441
			CTRL_BREAK_EVENT,
			/// <summary>
			/// The user pressed Ctrl+Close
			/// </summary>
			// Token: 0x040001BA RID: 442
			CTRL_CLOSE_EVENT,
			/// <summary>
			/// The user logged off
			/// </summary>
			// Token: 0x040001BB RID: 443
			CTRL_LOGOFF_EVENT = 5U,
			/// <summary>
			/// The computer shut dowm
			/// </summary>
			// Token: 0x040001BC RID: 444
			CTRL_SHUTDOWN_EVENT
		}

		/// <summary>
		/// A point.
		/// </summary>
		// Token: 0x020000CB RID: 203
		public struct POINT
		{
			/// <summary>
			/// Constructs a point
			/// </summary>
			/// <param name="x"></param>
			/// <param name="y"></param>
			// Token: 0x060004B4 RID: 1204 RVA: 0x00015E0F File Offset: 0x0001400F
			public POINT(int x, int y)
			{
				this.X = x;
				this.Y = y;
			}

			// Token: 0x040001BD RID: 445
			public int X;

			// Token: 0x040001BE RID: 446
			public int Y;
		}

		/// <summary>
		/// Windows Messages
		/// Defined in winuser.h from Windows SDK v6.1
		/// Documentation pulled from MSDN.
		/// </summary>
		// Token: 0x020000CC RID: 204
		public enum WM : uint
		{
			/// <summary>
			/// The WM_NULL message performs no operation. An application sends the WM_NULL message if it wants to post a message that the recipient window will ignore.
			/// </summary>
			// Token: 0x040001C0 RID: 448
			NULL,
			/// <summary>
			/// The WM_CREATE message is sent when an application requests that a window be created by calling the CreateWindowEx or CreateWindow function. (The message is sent before the function returns.) The window procedure of the new window receives this message after the window is created, but before the window becomes visible.
			/// </summary>
			// Token: 0x040001C1 RID: 449
			CREATE,
			/// <summary>
			/// The WM_DESTROY message is sent when a window is being destroyed. It is sent to the window procedure of the window being destroyed after the window is removed from the screen. 
			/// This message is sent first to the window being destroyed and then to the child windows (if any) as they are destroyed. During the processing of the message, it can be assumed that all child windows still exist.
			/// /// </summary>
			// Token: 0x040001C2 RID: 450
			DESTROY,
			/// <summary>
			/// The WM_MOVE message is sent after a window has been moved. 
			/// </summary>
			// Token: 0x040001C3 RID: 451
			MOVE,
			/// <summary>
			/// The WM_SIZE message is sent to a window after its size has changed.
			/// </summary>
			// Token: 0x040001C4 RID: 452
			SIZE = 5U,
			/// <summary>
			/// The WM_ACTIVATE message is sent to both the window being activated and the window being deactivated. If the windows use the same input queue, the message is sent synchronously, first to the window procedure of the top-level window being deactivated, then to the window procedure of the top-level window being activated. If the windows use different input queues, the message is sent asynchronously, so the window is activated immediately. 
			/// </summary>
			// Token: 0x040001C5 RID: 453
			ACTIVATE,
			/// <summary>
			/// The WM_SETFOCUS message is sent to a window after it has gained the keyboard focus. 
			/// </summary>
			// Token: 0x040001C6 RID: 454
			SETFOCUS,
			/// <summary>
			/// The WM_KILLFOCUS message is sent to a window immediately before it loses the keyboard focus. 
			/// </summary>
			// Token: 0x040001C7 RID: 455
			KILLFOCUS,
			/// <summary>
			/// The WM_ENABLE message is sent when an application changes the enabled state of a window. It is sent to the window whose enabled state is changing. This message is sent before the EnableWindow function returns, but after the enabled state (WS_DISABLED style bit) of the window has changed. 
			/// </summary>
			// Token: 0x040001C8 RID: 456
			ENABLE = 10U,
			/// <summary>
			/// An application sends the WM_SETREDRAW message to a window to allow changes in that window to be redrawn or to prevent changes in that window from being redrawn. 
			/// </summary>
			// Token: 0x040001C9 RID: 457
			SETREDRAW,
			/// <summary>
			/// An application sends a WM_SETTEXT message to set the text of a window. 
			/// </summary>
			// Token: 0x040001CA RID: 458
			SETTEXT,
			/// <summary>
			/// An application sends a WM_GETTEXT message to copy the text that corresponds to a window into a buffer provided by the caller. 
			/// </summary>
			// Token: 0x040001CB RID: 459
			GETTEXT,
			/// <summary>
			/// An application sends a WM_GETTEXTLENGTH message to determine the length, in characters, of the text associated with a window. 
			/// </summary>
			// Token: 0x040001CC RID: 460
			GETTEXTLENGTH,
			/// <summary>
			/// The WM_PAINT message is sent when the system or another application makes a request to paint a portion of an application's window. The message is sent when the UpdateWindow or RedrawWindow function is called, or by the DispatchMessage function when the application obtains a WM_PAINT message by using the GetMessage or PeekMessage function. 
			/// </summary>
			// Token: 0x040001CD RID: 461
			PAINT,
			/// <summary>
			/// The WM_CLOSE message is sent as a signal that a window or an application should terminate.
			/// </summary>
			// Token: 0x040001CE RID: 462
			CLOSE,
			/// <summary>
			/// The WM_QUERYENDSESSION message is sent when the user chooses to end the session or when an application calls one of the system shutdown functions. If any application returns zero, the session is not ended. The system stops sending WM_QUERYENDSESSION messages as soon as one application returns zero.
			/// After processing this message, the system sends the WM_ENDSESSION message with the wParam parameter set to the results of the WM_QUERYENDSESSION message.
			/// </summary>
			// Token: 0x040001CF RID: 463
			QUERYENDSESSION,
			/// <summary>
			/// The WM_QUERYOPEN message is sent to an icon when the user requests that the window be restored to its previous size and position.
			/// </summary>
			// Token: 0x040001D0 RID: 464
			QUERYOPEN = 19U,
			/// <summary>
			/// The WM_ENDSESSION message is sent to an application after the system processes the results of the WM_QUERYENDSESSION message. The WM_ENDSESSION message informs the application whether the session is ending.
			/// </summary>
			// Token: 0x040001D1 RID: 465
			ENDSESSION = 22U,
			/// <summary>
			/// The WM_QUIT message indicates a request to terminate an application and is generated when the application calls the PostQuitMessage function. It causes the GetMessage function to return zero.
			/// </summary>
			// Token: 0x040001D2 RID: 466
			QUIT = 18U,
			/// <summary>
			/// The WM_ERASEBKGND message is sent when the window background must be erased (for example, when a window is resized). The message is sent to prepare an invalidated portion of a window for painting. 
			/// </summary>
			// Token: 0x040001D3 RID: 467
			ERASEBKGND = 20U,
			/// <summary>
			/// This message is sent to all top-level windows when a change is made to a system color setting. 
			/// </summary>
			// Token: 0x040001D4 RID: 468
			SYSCOLORCHANGE,
			/// <summary>
			/// The WM_SHOWWINDOW message is sent to a window when the window is about to be hidden or shown.
			/// </summary>
			// Token: 0x040001D5 RID: 469
			SHOWWINDOW = 24U,
			/// <summary>
			/// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.
			/// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.
			/// </summary>
			// Token: 0x040001D6 RID: 470
			WININICHANGE = 26U,
			/// <summary>
			/// An application sends the WM_WININICHANGE message to all top-level windows after making a change to the WIN.INI file. The SystemParametersInfo function sends this message after an application uses the function to change a setting in WIN.INI.
			/// Note  The WM_WININICHANGE message is provided only for compatibility with earlier versions of the system. Applications should use the WM_SETTINGCHANGE message.
			/// </summary>
			// Token: 0x040001D7 RID: 471
			SETTINGCHANGE = 26U,
			/// <summary>
			/// The WM_DEVMODECHANGE message is sent to all top-level windows whenever the user changes device-mode settings. 
			/// </summary>
			// Token: 0x040001D8 RID: 472
			DEVMODECHANGE,
			/// <summary>
			/// The WM_ACTIVATEAPP message is sent when a window belonging to a different application than the active window is about to be activated. The message is sent to the application whose window is being activated and to the application whose window is being deactivated.
			/// </summary>
			// Token: 0x040001D9 RID: 473
			ACTIVATEAPP,
			/// <summary>
			/// An application sends the WM_FONTCHANGE message to all top-level windows in the system after changing the pool of font resources. 
			/// </summary>
			// Token: 0x040001DA RID: 474
			FONTCHANGE,
			/// <summary>
			/// A message that is sent whenever there is a change in the system time.
			/// </summary>
			// Token: 0x040001DB RID: 475
			TIMECHANGE,
			/// <summary>
			/// The WM_CANCELMODE message is sent to cancel certain modes, such as mouse capture. For example, the system sends this message to the active window when a dialog box or message box is displayed. Certain functions also send this message explicitly to the specified window regardless of whether it is the active window. For example, the EnableWindow function sends this message when disabling the specified window.
			/// </summary>
			// Token: 0x040001DC RID: 476
			CANCELMODE,
			/// <summary>
			/// The WM_SETCURSOR message is sent to a window if the mouse causes the cursor to move within a window and mouse input is not captured. 
			/// </summary>
			// Token: 0x040001DD RID: 477
			SETCURSOR,
			/// <summary>
			/// The WM_MOUSEACTIVATE message is sent when the cursor is in an inactive window and the user presses a mouse button. The parent window receives this message only if the child window passes it to the DefWindowProc function.
			/// </summary>
			// Token: 0x040001DE RID: 478
			MOUSEACTIVATE,
			/// <summary>
			/// The WM_CHILDACTIVATE message is sent to a child window when the user clicks the window's title bar or when the window is activated, moved, or sized.
			/// </summary>
			// Token: 0x040001DF RID: 479
			CHILDACTIVATE,
			/// <summary>
			/// The WM_QUEUESYNC message is sent by a computer-based training (CBT) application to separate user-input messages from other messages sent through the WH_JOURNALPLAYBACK Hook procedure. 
			/// </summary>
			// Token: 0x040001E0 RID: 480
			QUEUESYNC,
			/// <summary>
			/// The WM_GETMINMAXINFO message is sent to a window when the size or position of the window is about to change. An application can use this message to override the window's default maximized size and position, or its default minimum or maximum tracking size. 
			/// </summary>
			// Token: 0x040001E1 RID: 481
			GETMINMAXINFO,
			/// <summary>
			/// Windows NT 3.51 and earlier: The WM_PAINTICON message is sent to a minimized window when the icon is to be painted. This message is not sent by newer versions of Microsoft Windows, except in unusual circumstances explained in the Remarks.
			/// </summary>
			// Token: 0x040001E2 RID: 482
			PAINTICON = 38U,
			/// <summary>
			/// Windows NT 3.51 and earlier: The WM_ICONERASEBKGND message is sent to a minimized window when the background of the icon must be filled before painting the icon. A window receives this message only if a class icon is defined for the window; otherwise, WM_ERASEBKGND is sent. This message is not sent by newer versions of Windows.
			/// </summary>
			// Token: 0x040001E3 RID: 483
			ICONERASEBKGND,
			/// <summary>
			/// The WM_NEXTDLGCTL message is sent to a dialog box procedure to set the keyboard focus to a different control in the dialog box. 
			/// </summary>
			// Token: 0x040001E4 RID: 484
			NEXTDLGCTL,
			/// <summary>
			/// The WM_SPOOLERSTATUS message is sent from Print Manager whenever a job is added to or removed from the Print Manager queue. 
			/// </summary>
			// Token: 0x040001E5 RID: 485
			SPOOLERSTATUS = 42U,
			/// <summary>
			/// The WM_DRAWITEM message is sent to the parent window of an owner-drawn button, combo box, list box, or menu when a visual aspect of the button, combo box, list box, or menu has changed.
			/// </summary>
			// Token: 0x040001E6 RID: 486
			DRAWITEM,
			/// <summary>
			/// The WM_MEASUREITEM message is sent to the owner window of a combo box, list box, list view control, or menu item when the control or menu is created.
			/// </summary>
			// Token: 0x040001E7 RID: 487
			MEASUREITEM,
			/// <summary>
			/// Sent to the owner of a list box or combo box when the list box or combo box is destroyed or when items are removed by the LB_DELETESTRING, LB_RESETCONTENT, CB_DELETESTRING, or CB_RESETCONTENT message. The system sends a WM_DELETEITEM message for each deleted item. The system sends the WM_DELETEITEM message for any deleted list box or combo box item with nonzero item data.
			/// </summary>
			// Token: 0x040001E8 RID: 488
			DELETEITEM,
			/// <summary>
			/// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_KEYDOWN message. 
			/// </summary>
			// Token: 0x040001E9 RID: 489
			VKEYTOITEM,
			/// <summary>
			/// Sent by a list box with the LBS_WANTKEYBOARDINPUT style to its owner in response to a WM_CHAR message. 
			/// </summary>
			// Token: 0x040001EA RID: 490
			CHARTOITEM,
			/// <summary>
			/// An application sends a WM_SETFONT message to specify the font that a control is to use when drawing text. 
			/// </summary>
			// Token: 0x040001EB RID: 491
			SETFONT,
			/// <summary>
			/// An application sends a WM_GETFONT message to a control to retrieve the font with which the control is currently drawing its text. 
			/// </summary>
			// Token: 0x040001EC RID: 492
			GETFONT,
			/// <summary>
			/// An application sends a WM_SETHOTKEY message to a window to associate a hot key with the window. When the user presses the hot key, the system activates the window. 
			/// </summary>
			// Token: 0x040001ED RID: 493
			SETHOTKEY,
			/// <summary>
			/// An application sends a WM_GETHOTKEY message to determine the hot key associated with a window. 
			/// </summary>
			// Token: 0x040001EE RID: 494
			GETHOTKEY,
			/// <summary>
			/// The WM_QUERYDRAGICON message is sent to a minimized (iconic) window. The window is about to be dragged by the user but does not have an icon defined for its class. An application can return a handle to an icon or cursor. The system displays this cursor or icon while the user drags the icon.
			/// </summary>
			// Token: 0x040001EF RID: 495
			QUERYDRAGICON = 55U,
			/// <summary>
			/// The system sends the WM_COMPAREITEM message to determine the relative position of a new item in the sorted list of an owner-drawn combo box or list box. Whenever the application adds a new item, the system sends this message to the owner of a combo box or list box created with the CBS_SORT or LBS_SORT style. 
			/// </summary>
			// Token: 0x040001F0 RID: 496
			COMPAREITEM = 57U,
			/// <summary>
			/// Active Accessibility sends the WM_GETOBJECT message to obtain information about an accessible object contained in a server application. 
			/// Applications never send this message directly. It is sent only by Active Accessibility in response to calls to AccessibleObjectFromPoint, AccessibleObjectFromEvent, or AccessibleObjectFromWindow. However, server applications handle this message. 
			/// </summary>
			// Token: 0x040001F1 RID: 497
			GETOBJECT = 61U,
			/// <summary>
			/// The WM_COMPACTING message is sent to all top-level windows when the system detects more than 12.5 percent of system time over a 30- to 60-second interval is being spent compacting memory. This indicates that system memory is low.
			/// </summary>
			// Token: 0x040001F2 RID: 498
			COMPACTING = 65U,
			/// <summary>
			/// WM_COMMNOTIFY is Obsolete for Win32-Based Applications
			/// </summary>
			// Token: 0x040001F3 RID: 499
			[Obsolete]
			COMMNOTIFY = 68U,
			/// <summary>
			/// The WM_WINDOWPOSCHANGING message is sent to a window whose size, position, or place in the Z order is about to change as a result of a call to the SetWindowPos function or another window-management function.
			/// </summary>
			// Token: 0x040001F4 RID: 500
			WINDOWPOSCHANGING = 70U,
			/// <summary>
			/// The WM_WINDOWPOSCHANGED message is sent to a window whose size, position, or place in the Z order has changed as a result of a call to the SetWindowPos function or another window-management function.
			/// </summary>
			// Token: 0x040001F5 RID: 501
			WINDOWPOSCHANGED,
			/// <summary>
			/// Notifies applications that the system, typically a battery-powered personal computer, is about to enter a suspended mode.
			/// Use: POWERBROADCAST
			/// </summary>
			// Token: 0x040001F6 RID: 502
			[Obsolete]
			POWER,
			/// <summary>
			/// An application sends the WM_COPYDATA message to pass data to another application. 
			/// </summary>
			// Token: 0x040001F7 RID: 503
			COPYDATA = 74U,
			/// <summary>
			/// The WM_CANCELJOURNAL message is posted to an application when a user cancels the application's journaling activities. The message is posted with a NULL window handle. 
			/// </summary>
			// Token: 0x040001F8 RID: 504
			CANCELJOURNAL,
			/// <summary>
			/// Sent by a common control to its parent window when an event has occurred or the control requires some information. 
			/// </summary>
			// Token: 0x040001F9 RID: 505
			NOTIFY = 78U,
			/// <summary>
			/// The WM_INPUTLANGCHANGEREQUEST message is posted to the window with the focus when the user chooses a new input language, either with the hotkey (specified in the Keyboard control panel application) or from the indicator on the system taskbar. An application can accept the change by passing the message to the DefWindowProc function or reject the change (and prevent it from taking place) by returning immediately. 
			/// </summary>
			// Token: 0x040001FA RID: 506
			INPUTLANGCHANGEREQUEST = 80U,
			/// <summary>
			/// The WM_INPUTLANGCHANGE message is sent to the topmost affected window after an application's input language has been changed. You should make any application-specific settings and pass the message to the DefWindowProc function, which passes the message to all first-level child windows. These child windows can pass the message to DefWindowProc to have it pass the message to their child windows, and so on. 
			/// </summary>
			// Token: 0x040001FB RID: 507
			INPUTLANGCHANGE,
			/// <summary>
			/// Sent to an application that has initiated a training card with Microsoft Windows Help. The message informs the application when the user clicks an authorable button. An application initiates a training card by specifying the HELP_TCARD command in a call to the WinHelp function.
			/// </summary>
			// Token: 0x040001FC RID: 508
			TCARD,
			/// <summary>
			/// Indicates that the user pressed the F1 key. If a menu is active when F1 is pressed, WM_HELP is sent to the window associated with the menu; otherwise, WM_HELP is sent to the window that has the keyboard focus. If no window has the keyboard focus, WM_HELP is sent to the currently active window. 
			/// </summary>
			// Token: 0x040001FD RID: 509
			HELP,
			/// <summary>
			/// The WM_USERCHANGED message is sent to all windows after the user has logged on or off. When the user logs on or off, the system updates the user-specific settings. The system sends this message immediately after updating the settings.
			/// </summary>
			// Token: 0x040001FE RID: 510
			USERCHANGED,
			/// <summary>
			/// Determines if a window accepts ANSI or Unicode structures in the WM_NOTIFY notification message. WM_NOTIFYFORMAT messages are sent from a common control to its parent window and from the parent window to the common control.
			/// </summary>
			// Token: 0x040001FF RID: 511
			NOTIFYFORMAT,
			/// <summary>
			/// The WM_CONTEXTMENU message notifies a window that the user clicked the right mouse button (right-clicked) in the window.
			/// </summary>
			// Token: 0x04000200 RID: 512
			CONTEXTMENU = 123U,
			/// <summary>
			/// The WM_STYLECHANGING message is sent to a window when the SetWindowLong function is about to change one or more of the window's styles.
			/// </summary>
			// Token: 0x04000201 RID: 513
			STYLECHANGING,
			/// <summary>
			/// The WM_STYLECHANGED message is sent to a window after the SetWindowLong function has changed one or more of the window's styles
			/// </summary>
			// Token: 0x04000202 RID: 514
			STYLECHANGED,
			/// <summary>
			/// The WM_DISPLAYCHANGE message is sent to all windows when the display resolution has changed.
			/// </summary>
			// Token: 0x04000203 RID: 515
			DISPLAYCHANGE,
			/// <summary>
			/// The WM_GETICON message is sent to a window to retrieve a handle to the large or small icon associated with a window. The system displays the large icon in the ALT+TAB dialog, and the small icon in the window caption. 
			/// </summary>
			// Token: 0x04000204 RID: 516
			GETICON,
			/// <summary>
			/// An application sends the WM_SETICON message to associate a new large or small icon with a window. The system displays the large icon in the ALT+TAB dialog box, and the small icon in the window caption. 
			/// </summary>
			// Token: 0x04000205 RID: 517
			SETICON,
			/// <summary>
			/// The WM_NCCREATE message is sent prior to the WM_CREATE message when a window is first created.
			/// </summary>
			// Token: 0x04000206 RID: 518
			NCCREATE,
			/// <summary>
			/// The WM_NCDESTROY message informs a window that its nonclient area is being destroyed. The DestroyWindow function sends the WM_NCDESTROY message to the window following the WM_DESTROY message. WM_DESTROY is used to free the allocated memory object associated with the window. 
			/// The WM_NCDESTROY message is sent after the child windows have been destroyed. In contrast, WM_DESTROY is sent before the child windows are destroyed.
			/// </summary>
			// Token: 0x04000207 RID: 519
			NCDESTROY,
			/// <summary>
			/// The WM_NCCALCSIZE message is sent when the size and position of a window's client area must be calculated. By processing this message, an application can control the content of the window's client area when the size or position of the window changes.
			/// </summary>
			// Token: 0x04000208 RID: 520
			NCCALCSIZE,
			/// <summary>
			/// The WM_NCHITTEST message is sent to a window when the cursor moves, or when a mouse button is pressed or released. If the mouse is not captured, the message is sent to the window beneath the cursor. Otherwise, the message is sent to the window that has captured the mouse.
			/// </summary>
			// Token: 0x04000209 RID: 521
			NCHITTEST,
			/// <summary>
			/// The WM_NCPAINT message is sent to a window when its frame must be painted. 
			/// </summary>
			// Token: 0x0400020A RID: 522
			NCPAINT,
			/// <summary>
			/// The WM_NCACTIVATE message is sent to a window when its nonclient area needs to be changed to indicate an active or inactive state.
			/// </summary>
			// Token: 0x0400020B RID: 523
			NCACTIVATE,
			/// <summary>
			/// The WM_GETDLGCODE message is sent to the window procedure associated with a control. By default, the system handles all keyboard input to the control; the system interprets certain types of keyboard input as dialog box navigation keys. To override this default behavior, the control can respond to the WM_GETDLGCODE message to indicate the types of input it wants to process itself.
			/// </summary>
			// Token: 0x0400020C RID: 524
			GETDLGCODE,
			/// <summary>
			/// The WM_SYNCPAINT message is used to synchronize painting while avoiding linking independent GUI threads.
			/// </summary>
			// Token: 0x0400020D RID: 525
			SYNCPAINT,
			/// <summary>
			/// The WM_NCMOUSEMOVE message is posted to a window when the cursor is moved within the nonclient area of the window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
			/// </summary>
			// Token: 0x0400020E RID: 526
			NCMOUSEMOVE = 160U,
			/// <summary>
			/// The WM_NCLBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
			/// </summary>
			// Token: 0x0400020F RID: 527
			NCLBUTTONDOWN,
			/// <summary>
			/// The WM_NCLBUTTONUP message is posted when the user releases the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
			/// </summary>
			// Token: 0x04000210 RID: 528
			NCLBUTTONUP,
			/// <summary>
			/// The WM_NCLBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
			/// </summary>
			// Token: 0x04000211 RID: 529
			NCLBUTTONDBLCLK,
			/// <summary>
			/// The WM_NCRBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
			/// </summary>
			// Token: 0x04000212 RID: 530
			NCRBUTTONDOWN,
			/// <summary>
			/// The WM_NCRBUTTONUP message is posted when the user releases the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
			/// </summary>
			// Token: 0x04000213 RID: 531
			NCRBUTTONUP,
			/// <summary>
			/// The WM_NCRBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
			/// </summary>
			// Token: 0x04000214 RID: 532
			NCRBUTTONDBLCLK,
			/// <summary>
			/// The WM_NCMBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
			/// </summary>
			// Token: 0x04000215 RID: 533
			NCMBUTTONDOWN,
			/// <summary>
			/// The WM_NCMBUTTONUP message is posted when the user releases the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
			/// </summary>
			// Token: 0x04000216 RID: 534
			NCMBUTTONUP,
			/// <summary>
			/// The WM_NCMBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is within the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
			/// </summary>
			// Token: 0x04000217 RID: 535
			NCMBUTTONDBLCLK,
			/// <summary>
			/// The WM_NCXBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
			/// </summary>
			// Token: 0x04000218 RID: 536
			NCXBUTTONDOWN = 171U,
			/// <summary>
			/// The WM_NCXBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
			/// </summary>
			// Token: 0x04000219 RID: 537
			NCXBUTTONUP,
			/// <summary>
			/// The WM_NCXBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the nonclient area of a window. This message is posted to the window that contains the cursor. If a window has captured the mouse, this message is not posted.
			/// </summary>
			// Token: 0x0400021A RID: 538
			NCXBUTTONDBLCLK,
			/// <summary>
			/// The WM_INPUT_DEVICE_CHANGE message is sent to the window that registered to receive raw input. A window receives this message through its WindowProc function.
			/// </summary>
			// Token: 0x0400021B RID: 539
			INPUT_DEVICE_CHANGE = 254U,
			/// <summary>
			/// The WM_INPUT message is sent to the window that is getting raw input. 
			/// </summary>
			// Token: 0x0400021C RID: 540
			INPUT,
			/// <summary>
			/// This message filters for keyboard messages.
			/// </summary>
			// Token: 0x0400021D RID: 541
			KEYFIRST,
			/// <summary>
			/// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed. 
			/// </summary>
			// Token: 0x0400021E RID: 542
			KEYDOWN = 256U,
			/// <summary>
			/// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, or a keyboard key that is pressed when a window has the keyboard focus. 
			/// </summary>
			// Token: 0x0400021F RID: 543
			KEYUP,
			/// <summary>
			/// The WM_CHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_CHAR message contains the character code of the key that was pressed. 
			/// </summary>
			// Token: 0x04000220 RID: 544
			CHAR,
			/// <summary>
			/// The WM_DEADCHAR message is posted to the window with the keyboard focus when a WM_KEYUP message is translated by the TranslateMessage function. WM_DEADCHAR specifies a character code generated by a dead key. A dead key is a key that generates a character, such as the umlaut (double-dot), that is combined with another character to form a composite character. For example, the umlaut-O character (Ö) is generated by typing the dead key for the umlaut character, and then typing the O key. 
			/// </summary>
			// Token: 0x04000221 RID: 545
			DEADCHAR,
			/// <summary>
			/// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user presses the F10 key (which activates the menu bar) or holds down the ALT key and then presses another key. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter. 
			/// </summary>
			// Token: 0x04000222 RID: 546
			SYSKEYDOWN,
			/// <summary>
			/// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user releases a key that was pressed while the ALT key was held down. It also occurs when no window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent to the active window. The window that receives the message can distinguish between these two contexts by checking the context code in the lParam parameter. 
			/// </summary>
			// Token: 0x04000223 RID: 547
			SYSKEYUP,
			/// <summary>
			/// The WM_SYSCHAR message is posted to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. It specifies the character code of a system character key — that is, a character key that is pressed while the ALT key is down. 
			/// </summary>
			// Token: 0x04000224 RID: 548
			SYSCHAR,
			/// <summary>
			/// The WM_SYSDEADCHAR message is sent to the window with the keyboard focus when a WM_SYSKEYDOWN message is translated by the TranslateMessage function. WM_SYSDEADCHAR specifies the character code of a system dead key — that is, a dead key that is pressed while holding down the ALT key. 
			/// </summary>
			// Token: 0x04000225 RID: 549
			SYSDEADCHAR,
			/// <summary>
			/// The WM_UNICHAR message is posted to the window with the keyboard focus when a WM_KEYDOWN message is translated by the TranslateMessage function. The WM_UNICHAR message contains the character code of the key that was pressed. 
			/// The WM_UNICHAR message is equivalent to WM_CHAR, but it uses Unicode Transformation Format (UTF)-32, whereas WM_CHAR uses UTF-16. It is designed to send or post Unicode characters to ANSI windows and it can can handle Unicode Supplementary Plane characters.
			/// </summary>
			// Token: 0x04000226 RID: 550
			UNICHAR = 265U,
			/// <summary>
			/// This message filters for keyboard messages.
			/// </summary>
			// Token: 0x04000227 RID: 551
			KEYLAST = 264U,
			/// <summary>
			/// Sent immediately before the IME generates the composition string as a result of a keystroke. A window receives this message through its WindowProc function. 
			/// </summary>
			// Token: 0x04000228 RID: 552
			IME_STARTCOMPOSITION = 269U,
			/// <summary>
			/// Sent to an application when the IME ends composition. A window receives this message through its WindowProc function. 
			/// </summary>
			// Token: 0x04000229 RID: 553
			IME_ENDCOMPOSITION,
			/// <summary>
			/// Sent to an application when the IME changes composition status as a result of a keystroke. A window receives this message through its WindowProc function. 
			/// </summary>
			// Token: 0x0400022A RID: 554
			IME_COMPOSITION,
			// Token: 0x0400022B RID: 555
			IME_KEYLAST = 271U,
			/// <summary>
			/// The WM_INITDIALOG message is sent to the dialog box procedure immediately before a dialog box is displayed. Dialog box procedures typically use this message to initialize controls and carry out any other initialization tasks that affect the appearance of the dialog box. 
			/// </summary>
			// Token: 0x0400022C RID: 556
			INITDIALOG,
			/// <summary>
			/// The WM_COMMAND message is sent when the user selects a command item from a menu, when a control sends a notification message to its parent window, or when an accelerator keystroke is translated. 
			/// </summary>
			// Token: 0x0400022D RID: 557
			COMMAND,
			/// <summary>
			/// A window receives this message when the user chooses a command from the Window menu, clicks the maximize button, minimize button, restore button, close button, or moves the form. You can stop the form from moving by filtering this out.
			/// </summary>
			// Token: 0x0400022E RID: 558
			SYSCOMMAND,
			/// <summary>
			/// The WM_TIMER message is posted to the installing thread's message queue when a timer expires. The message is posted by the GetMessage or PeekMessage function. 
			/// </summary>
			// Token: 0x0400022F RID: 559
			TIMER,
			/// <summary>
			/// The WM_HSCROLL message is sent to a window when a scroll event occurs in the window's standard horizontal scroll bar. This message is also sent to the owner of a horizontal scroll bar control when a scroll event occurs in the control. 
			/// </summary>
			// Token: 0x04000230 RID: 560
			HSCROLL,
			/// <summary>
			/// The WM_VSCROLL message is sent to a window when a scroll event occurs in the window's standard vertical scroll bar. This message is also sent to the owner of a vertical scroll bar control when a scroll event occurs in the control. 
			/// </summary>
			// Token: 0x04000231 RID: 561
			VSCROLL,
			/// <summary>
			/// The WM_INITMENU message is sent when a menu is about to become active. It occurs when the user clicks an item on the menu bar or presses a menu key. This allows the application to modify the menu before it is displayed. 
			/// </summary>
			// Token: 0x04000232 RID: 562
			INITMENU,
			/// <summary>
			/// The WM_INITMENUPOPUP message is sent when a drop-down menu or submenu is about to become active. This allows an application to modify the menu before it is displayed, without changing the entire menu. 
			/// </summary>
			// Token: 0x04000233 RID: 563
			INITMENUPOPUP,
			/// <summary>
			/// The WM_MENUSELECT message is sent to a menu's owner window when the user selects a menu item. 
			/// </summary>
			// Token: 0x04000234 RID: 564
			MENUSELECT = 287U,
			/// <summary>
			/// The WM_MENUCHAR message is sent when a menu is active and the user presses a key that does not correspond to any mnemonic or accelerator key. This message is sent to the window that owns the menu. 
			/// </summary>
			// Token: 0x04000235 RID: 565
			MENUCHAR,
			/// <summary>
			/// The WM_ENTERIDLE message is sent to the owner window of a modal dialog box or menu that is entering an idle state. A modal dialog box or menu enters an idle state when no messages are waiting in its queue after it has processed one or more previous messages. 
			/// </summary>
			// Token: 0x04000236 RID: 566
			ENTERIDLE,
			/// <summary>
			/// The WM_MENURBUTTONUP message is sent when the user releases the right mouse button while the cursor is on a menu item. 
			/// </summary>
			// Token: 0x04000237 RID: 567
			MENURBUTTONUP,
			/// <summary>
			/// The WM_MENUDRAG message is sent to the owner of a drag-and-drop menu when the user drags a menu item. 
			/// </summary>
			// Token: 0x04000238 RID: 568
			MENUDRAG,
			/// <summary>
			/// The WM_MENUGETOBJECT message is sent to the owner of a drag-and-drop menu when the mouse cursor enters a menu item or moves from the center of the item to the top or bottom of the item. 
			/// </summary>
			// Token: 0x04000239 RID: 569
			MENUGETOBJECT,
			/// <summary>
			/// The WM_UNINITMENUPOPUP message is sent when a drop-down menu or submenu has been destroyed. 
			/// </summary>
			// Token: 0x0400023A RID: 570
			UNINITMENUPOPUP,
			/// <summary>
			/// The WM_MENUCOMMAND message is sent when the user makes a selection from a menu. 
			/// </summary>
			// Token: 0x0400023B RID: 571
			MENUCOMMAND,
			/// <summary>
			/// An application sends the WM_CHANGEUISTATE message to indicate that the user interface (UI) state should be changed.
			/// </summary>
			// Token: 0x0400023C RID: 572
			CHANGEUISTATE,
			/// <summary>
			/// An application sends the WM_UPDATEUISTATE message to change the user interface (UI) state for the specified window and all its child windows.
			/// </summary>
			// Token: 0x0400023D RID: 573
			UPDATEUISTATE,
			/// <summary>
			/// An application sends the WM_QUERYUISTATE message to retrieve the user interface (UI) state for a window.
			/// </summary>
			// Token: 0x0400023E RID: 574
			QUERYUISTATE,
			/// <summary>
			/// The WM_CTLCOLORMSGBOX message is sent to the owner window of a message box before Windows draws the message box. By responding to this message, the owner window can set the text and background colors of the message box by using the given display device context handle. 
			/// </summary>
			// Token: 0x0400023F RID: 575
			CTLCOLORMSGBOX = 306U,
			/// <summary>
			/// An edit control that is not read-only or disabled sends the WM_CTLCOLOREDIT message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the edit control. 
			/// </summary>
			// Token: 0x04000240 RID: 576
			CTLCOLOREDIT,
			/// <summary>
			/// Sent to the parent window of a list box before the system draws the list box. By responding to this message, the parent window can set the text and background colors of the list box by using the specified display device context handle. 
			/// </summary>
			// Token: 0x04000241 RID: 577
			CTLCOLORLISTBOX,
			/// <summary>
			/// The WM_CTLCOLORBTN message is sent to the parent window of a button before drawing the button. The parent window can change the button's text and background colors. However, only owner-drawn buttons respond to the parent window processing this message. 
			/// </summary>
			// Token: 0x04000242 RID: 578
			CTLCOLORBTN,
			/// <summary>
			/// The WM_CTLCOLORDLG message is sent to a dialog box before the system draws the dialog box. By responding to this message, the dialog box can set its text and background colors using the specified display device context handle. 
			/// </summary>
			// Token: 0x04000243 RID: 579
			CTLCOLORDLG,
			/// <summary>
			/// The WM_CTLCOLORSCROLLBAR message is sent to the parent window of a scroll bar control when the control is about to be drawn. By responding to this message, the parent window can use the display context handle to set the background color of the scroll bar control. 
			/// </summary>
			// Token: 0x04000244 RID: 580
			CTLCOLORSCROLLBAR,
			/// <summary>
			/// A static control, or an edit control that is read-only or disabled, sends the WM_CTLCOLORSTATIC message to its parent window when the control is about to be drawn. By responding to this message, the parent window can use the specified device context handle to set the text and background colors of the static control. 
			/// </summary>
			// Token: 0x04000245 RID: 581
			CTLCOLORSTATIC,
			/// <summary>
			/// Use WM_MOUSEFIRST to specify the first mouse message. Use the PeekMessage() Function.
			/// </summary>
			// Token: 0x04000246 RID: 582
			MOUSEFIRST = 512U,
			/// <summary>
			/// The WM_MOUSEMOVE message is posted to a window when the cursor moves. If the mouse is not captured, the message is posted to the window that contains the cursor. Otherwise, the message is posted to the window that has captured the mouse.
			/// </summary>
			// Token: 0x04000247 RID: 583
			MOUSEMOVE = 512U,
			/// <summary>
			/// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
			/// </summary>
			// Token: 0x04000248 RID: 584
			LBUTTONDOWN,
			/// <summary>
			/// The WM_LBUTTONUP message is posted when the user releases the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
			/// </summary>
			// Token: 0x04000249 RID: 585
			LBUTTONUP,
			/// <summary>
			/// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
			/// </summary>
			// Token: 0x0400024A RID: 586
			LBUTTONDBLCLK,
			/// <summary>
			/// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
			/// </summary>
			// Token: 0x0400024B RID: 587
			RBUTTONDOWN,
			/// <summary>
			/// The WM_RBUTTONUP message is posted when the user releases the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
			/// </summary>
			// Token: 0x0400024C RID: 588
			RBUTTONUP,
			/// <summary>
			/// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
			/// </summary>
			// Token: 0x0400024D RID: 589
			RBUTTONDBLCLK,
			/// <summary>
			/// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
			/// </summary>
			// Token: 0x0400024E RID: 590
			MBUTTONDOWN,
			/// <summary>
			/// The WM_MBUTTONUP message is posted when the user releases the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
			/// </summary>
			// Token: 0x0400024F RID: 591
			MBUTTONUP,
			/// <summary>
			/// The WM_MBUTTONDBLCLK message is posted when the user double-clicks the middle mouse button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
			/// </summary>
			// Token: 0x04000250 RID: 592
			MBUTTONDBLCLK,
			/// <summary>
			/// The WM_MOUSEWHEEL message is sent to the focus window when the mouse wheel is rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
			/// </summary>
			// Token: 0x04000251 RID: 593
			MOUSEWHEEL,
			/// <summary>
			/// The WM_XBUTTONDOWN message is posted when the user presses the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse. 
			/// </summary>
			// Token: 0x04000252 RID: 594
			XBUTTONDOWN,
			/// <summary>
			/// The WM_XBUTTONUP message is posted when the user releases the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
			/// </summary>
			// Token: 0x04000253 RID: 595
			XBUTTONUP,
			/// <summary>
			/// The WM_XBUTTONDBLCLK message is posted when the user double-clicks the first or second X button while the cursor is in the client area of a window. If the mouse is not captured, the message is posted to the window beneath the cursor. Otherwise, the message is posted to the window that has captured the mouse.
			/// </summary>
			// Token: 0x04000254 RID: 596
			XBUTTONDBLCLK,
			/// <summary>
			/// The WM_MOUSEHWHEEL message is sent to the focus window when the mouse's horizontal scroll wheel is tilted or rotated. The DefWindowProc function propagates the message to the window's parent. There should be no internal forwarding of the message, since DefWindowProc propagates it up the parent chain until it finds a window that processes it.
			/// </summary>
			// Token: 0x04000255 RID: 597
			MOUSEHWHEEL,
			/// <summary>
			/// Use WM_MOUSELAST to specify the last mouse message. Used with PeekMessage() Function.
			/// </summary>
			// Token: 0x04000256 RID: 598
			MOUSELAST = 526U,
			/// <summary>
			/// The WM_PARENTNOTIFY message is sent to the parent of a child window when the child window is created or destroyed, or when the user clicks a mouse button while the cursor is over the child window. When the child window is being created, the system sends WM_PARENTNOTIFY just before the CreateWindow or CreateWindowEx function that creates the window returns. When the child window is being destroyed, the system sends the message before any processing to destroy the window takes place.
			/// </summary>
			// Token: 0x04000257 RID: 599
			PARENTNOTIFY = 528U,
			/// <summary>
			/// The WM_ENTERMENULOOP message informs an application's main window procedure that a menu modal loop has been entered. 
			/// </summary>
			// Token: 0x04000258 RID: 600
			ENTERMENULOOP,
			/// <summary>
			/// The WM_EXITMENULOOP message informs an application's main window procedure that a menu modal loop has been exited. 
			/// </summary>
			// Token: 0x04000259 RID: 601
			EXITMENULOOP,
			/// <summary>
			/// The WM_NEXTMENU message is sent to an application when the right or left arrow key is used to switch between the menu bar and the system menu. 
			/// </summary>
			// Token: 0x0400025A RID: 602
			NEXTMENU,
			/// <summary>
			/// The WM_SIZING message is sent to a window that the user is resizing. By processing this message, an application can monitor the size and position of the drag rectangle and, if needed, change its size or position. 
			/// </summary>
			// Token: 0x0400025B RID: 603
			SIZING,
			/// <summary>
			/// The WM_CAPTURECHANGED message is sent to the window that is losing the mouse capture.
			/// </summary>
			// Token: 0x0400025C RID: 604
			CAPTURECHANGED,
			/// <summary>
			/// The WM_MOVING message is sent to a window that the user is moving. By processing this message, an application can monitor the position of the drag rectangle and, if needed, change its position.
			/// </summary>
			// Token: 0x0400025D RID: 605
			MOVING,
			/// <summary>
			/// Notifies applications that a power-management event has occurred.
			/// </summary>
			// Token: 0x0400025E RID: 606
			POWERBROADCAST = 536U,
			/// <summary>
			/// Notifies an application of a change to the hardware configuration of a device or the computer.
			/// </summary>
			// Token: 0x0400025F RID: 607
			DEVICECHANGE,
			/// <summary>
			/// An application sends the WM_MDICREATE message to a multiple-document interface (MDI) client window to create an MDI child window. 
			/// </summary>
			// Token: 0x04000260 RID: 608
			MDICREATE = 544U,
			/// <summary>
			/// An application sends the WM_MDIDESTROY message to a multiple-document interface (MDI) client window to close an MDI child window. 
			/// </summary>
			// Token: 0x04000261 RID: 609
			MDIDESTROY,
			/// <summary>
			/// An application sends the WM_MDIACTIVATE message to a multiple-document interface (MDI) client window to instruct the client window to activate a different MDI child window. 
			/// </summary>
			// Token: 0x04000262 RID: 610
			MDIACTIVATE,
			/// <summary>
			/// An application sends the WM_MDIRESTORE message to a multiple-document interface (MDI) client window to restore an MDI child window from maximized or minimized size. 
			/// </summary>
			// Token: 0x04000263 RID: 611
			MDIRESTORE,
			/// <summary>
			/// An application sends the WM_MDINEXT message to a multiple-document interface (MDI) client window to activate the next or previous child window. 
			/// </summary>
			// Token: 0x04000264 RID: 612
			MDINEXT,
			/// <summary>
			/// An application sends the WM_MDIMAXIMIZE message to a multiple-document interface (MDI) client window to maximize an MDI child window. The system resizes the child window to make its client area fill the client window. The system places the child window's window menu icon in the rightmost position of the frame window's menu bar, and places the child window's restore icon in the leftmost position. The system also appends the title bar text of the child window to that of the frame window. 
			/// </summary>
			// Token: 0x04000265 RID: 613
			MDIMAXIMIZE,
			/// <summary>
			/// An application sends the WM_MDITILE message to a multiple-document interface (MDI) client window to arrange all of its MDI child windows in a tile format. 
			/// </summary>
			// Token: 0x04000266 RID: 614
			MDITILE,
			/// <summary>
			/// An application sends the WM_MDICASCADE message to a multiple-document interface (MDI) client window to arrange all its child windows in a cascade format. 
			/// </summary>
			// Token: 0x04000267 RID: 615
			MDICASCADE,
			/// <summary>
			/// An application sends the WM_MDIICONARRANGE message to a multiple-document interface (MDI) client window to arrange all minimized MDI child windows. It does not affect child windows that are not minimized. 
			/// </summary>
			// Token: 0x04000268 RID: 616
			MDIICONARRANGE,
			/// <summary>
			/// An application sends the WM_MDIGETACTIVE message to a multiple-document interface (MDI) client window to retrieve the handle to the active MDI child window. 
			/// </summary>
			// Token: 0x04000269 RID: 617
			MDIGETACTIVE,
			/// <summary>
			/// An application sends the WM_MDISETMENU message to a multiple-document interface (MDI) client window to replace the entire menu of an MDI frame window, to replace the window menu of the frame window, or both. 
			/// </summary>
			// Token: 0x0400026A RID: 618
			MDISETMENU = 560U,
			/// <summary>
			/// The WM_ENTERSIZEMOVE message is sent one time to a window after it enters the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns. 
			/// The system sends the WM_ENTERSIZEMOVE message regardless of whether the dragging of full windows is enabled.
			/// </summary>
			// Token: 0x0400026B RID: 619
			ENTERSIZEMOVE,
			/// <summary>
			/// The WM_EXITSIZEMOVE message is sent one time to a window, after it has exited the moving or sizing modal loop. The window enters the moving or sizing modal loop when the user clicks the window's title bar or sizing border, or when the window passes the WM_SYSCOMMAND message to the DefWindowProc function and the wParam parameter of the message specifies the SC_MOVE or SC_SIZE value. The operation is complete when DefWindowProc returns. 
			/// </summary>
			// Token: 0x0400026C RID: 620
			EXITSIZEMOVE,
			/// <summary>
			/// Sent when the user drops a file on the window of an application that has registered itself as a recipient of dropped files.
			/// </summary>
			// Token: 0x0400026D RID: 621
			DROPFILES,
			/// <summary>
			/// An application sends the WM_MDIREFRESHMENU message to a multiple-document interface (MDI) client window to refresh the window menu of the MDI frame window. 
			/// </summary>
			// Token: 0x0400026E RID: 622
			MDIREFRESHMENU,
			/// <summary>
			/// Sent to an application when a window is activated. A window receives this message through its WindowProc function. 
			/// </summary>
			// Token: 0x0400026F RID: 623
			IME_SETCONTEXT = 641U,
			/// <summary>
			/// Sent to an application to notify it of changes to the IME window. A window receives this message through its WindowProc function. 
			/// </summary>
			// Token: 0x04000270 RID: 624
			IME_NOTIFY,
			/// <summary>
			/// Sent by an application to direct the IME window to carry out the requested command. The application uses this message to control the IME window that it has created. To send this message, the application calls the SendMessage function with the following parameters.
			/// </summary>
			// Token: 0x04000271 RID: 625
			IME_CONTROL,
			/// <summary>
			/// Sent to an application when the IME window finds no space to extend the area for the composition window. A window receives this message through its WindowProc function. 
			/// </summary>
			// Token: 0x04000272 RID: 626
			IME_COMPOSITIONFULL,
			/// <summary>
			/// Sent to an application when the operating system is about to change the current IME. A window receives this message through its WindowProc function. 
			/// </summary>
			// Token: 0x04000273 RID: 627
			IME_SELECT,
			/// <summary>
			/// Sent to an application when the IME gets a character of the conversion result. A window receives this message through its WindowProc function. 
			/// </summary>
			// Token: 0x04000274 RID: 628
			IME_CHAR,
			/// <summary>
			/// Sent to an application to provide commands and request information. A window receives this message through its WindowProc function. 
			/// </summary>
			// Token: 0x04000275 RID: 629
			IME_REQUEST = 648U,
			/// <summary>
			/// Sent to an application by the IME to notify the application of a key press and to keep message order. A window receives this message through its WindowProc function. 
			/// </summary>
			// Token: 0x04000276 RID: 630
			IME_KEYDOWN = 656U,
			/// <summary>
			/// Sent to an application by the IME to notify the application of a key release and to keep message order. A window receives this message through its WindowProc function. 
			/// </summary>
			// Token: 0x04000277 RID: 631
			IME_KEYUP,
			/// <summary>
			/// The WM_MOUSEHOVER message is posted to a window when the cursor hovers over the client area of the window for the period of time specified in a prior call to TrackMouseEvent.
			/// </summary>
			// Token: 0x04000278 RID: 632
			MOUSEHOVER = 673U,
			/// <summary>
			/// The WM_MOUSELEAVE message is posted to a window when the cursor leaves the client area of the window specified in a prior call to TrackMouseEvent.
			/// </summary>
			// Token: 0x04000279 RID: 633
			MOUSELEAVE = 675U,
			/// <summary>
			/// The WM_NCMOUSEHOVER message is posted to a window when the cursor hovers over the nonclient area of the window for the period of time specified in a prior call to TrackMouseEvent.
			/// </summary>
			// Token: 0x0400027A RID: 634
			NCMOUSEHOVER = 672U,
			/// <summary>
			/// The WM_NCMOUSELEAVE message is posted to a window when the cursor leaves the nonclient area of the window specified in a prior call to TrackMouseEvent.
			/// </summary>
			// Token: 0x0400027B RID: 635
			NCMOUSELEAVE = 674U,
			/// <summary>
			/// The WM_WTSSESSION_CHANGE message notifies applications of changes in session state.
			/// </summary>
			// Token: 0x0400027C RID: 636
			WTSSESSION_CHANGE = 689U,
			// Token: 0x0400027D RID: 637
			TABLET_FIRST = 704U,
			// Token: 0x0400027E RID: 638
			TABLET_LAST = 735U,
			/// <summary>
			/// An application sends a WM_CUT message to an edit control or combo box to delete (cut) the current selection, if any, in the edit control and copy the deleted text to the clipboard in CF_TEXT format. 
			/// </summary>
			// Token: 0x0400027F RID: 639
			CUT = 768U,
			/// <summary>
			/// An application sends the WM_COPY message to an edit control or combo box to copy the current selection to the clipboard in CF_TEXT format. 
			/// </summary>
			// Token: 0x04000280 RID: 640
			COPY,
			/// <summary>
			/// An application sends a WM_PASTE message to an edit control or combo box to copy the current content of the clipboard to the edit control at the current caret position. Data is inserted only if the clipboard contains data in CF_TEXT format. 
			/// </summary>
			// Token: 0x04000281 RID: 641
			PASTE,
			/// <summary>
			/// An application sends a WM_CLEAR message to an edit control or combo box to delete (clear) the current selection, if any, from the edit control. 
			/// </summary>
			// Token: 0x04000282 RID: 642
			CLEAR,
			/// <summary>
			/// An application sends a WM_UNDO message to an edit control to undo the last operation. When this message is sent to an edit control, the previously deleted text is restored or the previously added text is deleted.
			/// </summary>
			// Token: 0x04000283 RID: 643
			UNDO,
			/// <summary>
			/// The WM_RENDERFORMAT message is sent to the clipboard owner if it has delayed rendering a specific clipboard format and if an application has requested data in that format. The clipboard owner must render data in the specified format and place it on the clipboard by calling the SetClipboardData function. 
			/// </summary>
			// Token: 0x04000284 RID: 644
			RENDERFORMAT,
			/// <summary>
			/// The WM_RENDERALLFORMATS message is sent to the clipboard owner before it is destroyed, if the clipboard owner has delayed rendering one or more clipboard formats. For the content of the clipboard to remain available to other applications, the clipboard owner must render data in all the formats it is capable of generating, and place the data on the clipboard by calling the SetClipboardData function. 
			/// </summary>
			// Token: 0x04000285 RID: 645
			RENDERALLFORMATS,
			/// <summary>
			/// The WM_DESTROYCLIPBOARD message is sent to the clipboard owner when a call to the EmptyClipboard function empties the clipboard. 
			/// </summary>
			// Token: 0x04000286 RID: 646
			DESTROYCLIPBOARD,
			/// <summary>
			/// The WM_DRAWCLIPBOARD message is sent to the first window in the clipboard viewer chain when the content of the clipboard changes. This enables a clipboard viewer window to display the new content of the clipboard. 
			/// </summary>
			// Token: 0x04000287 RID: 647
			DRAWCLIPBOARD,
			/// <summary>
			/// The WM_PAINTCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area needs repainting. 
			/// </summary>
			// Token: 0x04000288 RID: 648
			PAINTCLIPBOARD,
			/// <summary>
			/// The WM_VSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's vertical scroll bar. The owner should scroll the clipboard image and update the scroll bar values. 
			/// </summary>
			// Token: 0x04000289 RID: 649
			VSCROLLCLIPBOARD,
			/// <summary>
			/// The WM_SIZECLIPBOARD message is sent to the clipboard owner by a clipboard viewer window when the clipboard contains data in the CF_OWNERDISPLAY format and the clipboard viewer's client area has changed size. 
			/// </summary>
			// Token: 0x0400028A RID: 650
			SIZECLIPBOARD,
			/// <summary>
			/// The WM_ASKCBFORMATNAME message is sent to the clipboard owner by a clipboard viewer window to request the name of a CF_OWNERDISPLAY clipboard format.
			/// </summary>
			// Token: 0x0400028B RID: 651
			ASKCBFORMATNAME,
			/// <summary>
			/// The WM_CHANGECBCHAIN message is sent to the first window in the clipboard viewer chain when a window is being removed from the chain. 
			/// </summary>
			// Token: 0x0400028C RID: 652
			CHANGECBCHAIN,
			/// <summary>
			/// The WM_HSCROLLCLIPBOARD message is sent to the clipboard owner by a clipboard viewer window. This occurs when the clipboard contains data in the CF_OWNERDISPLAY format and an event occurs in the clipboard viewer's horizontal scroll bar. The owner should scroll the clipboard image and update the scroll bar values. 
			/// </summary>
			// Token: 0x0400028D RID: 653
			HSCROLLCLIPBOARD,
			/// <summary>
			/// This message informs a window that it is about to receive the keyboard focus, giving the window the opportunity to realize its logical palette when it receives the focus. 
			/// </summary>
			// Token: 0x0400028E RID: 654
			QUERYNEWPALETTE,
			/// <summary>
			/// The WM_PALETTEISCHANGING message informs applications that an application is going to realize its logical palette. 
			/// </summary>
			// Token: 0x0400028F RID: 655
			PALETTEISCHANGING,
			/// <summary>
			/// This message is sent by the OS to all top-level and overlapped windows after the window with the keyboard focus realizes its logical palette. 
			/// This message enables windows that do not have the keyboard focus to realize their logical palettes and update their client areas.
			/// </summary>
			// Token: 0x04000290 RID: 656
			PALETTECHANGED,
			/// <summary>
			/// The WM_HOTKEY message is posted when the user presses a hot key registered by the RegisterHotKey function. The message is placed at the top of the message queue associated with the thread that registered the hot key. 
			/// </summary>
			// Token: 0x04000291 RID: 657
			HOTKEY,
			/// <summary>
			/// The WM_PRINT message is sent to a window to request that it draw itself in the specified device context, most commonly in a printer device context.
			/// </summary>
			// Token: 0x04000292 RID: 658
			PRINT = 791U,
			/// <summary>
			/// The WM_PRINTCLIENT message is sent to a window to request that it draw its client area in the specified device context, most commonly in a printer device context.
			/// </summary>
			// Token: 0x04000293 RID: 659
			PRINTCLIENT,
			/// <summary>
			/// The WM_APPCOMMAND message notifies a window that the user generated an application command event, for example, by clicking an application command button using the mouse or typing an application command key on the keyboard.
			/// </summary>
			// Token: 0x04000294 RID: 660
			APPCOMMAND,
			/// <summary>
			/// The WM_THEMECHANGED message is broadcast to every window following a theme change event. Examples of theme change events are the activation of a theme, the deactivation of a theme, or a transition from one theme to another.
			/// </summary>
			// Token: 0x04000295 RID: 661
			THEMECHANGED,
			/// <summary>
			/// Sent when the contents of the clipboard have changed.
			/// </summary>
			// Token: 0x04000296 RID: 662
			CLIPBOARDUPDATE = 797U,
			/// <summary>
			/// The system will send a window the WM_DWMCOMPOSITIONCHANGED message to indicate that the availability of desktop composition has changed.
			/// </summary>
			// Token: 0x04000297 RID: 663
			DWMCOMPOSITIONCHANGED,
			/// <summary>
			/// WM_DWMNCRENDERINGCHANGED is called when the non-client area rendering status of a window has changed. Only windows that have set the flag DWM_BLURBEHIND.fTransitionOnMaximized to true will get this message. 
			/// </summary>
			// Token: 0x04000298 RID: 664
			DWMNCRENDERINGCHANGED,
			/// <summary>
			/// Sent to all top-level windows when the colorization color has changed. 
			/// </summary>
			// Token: 0x04000299 RID: 665
			DWMCOLORIZATIONCOLORCHANGED,
			/// <summary>
			/// WM_DWMWINDOWMAXIMIZEDCHANGE will let you know when a DWM composed window is maximized. You also have to register for this message as well. You'd have other windowd go opaque when this message is sent.
			/// </summary>
			// Token: 0x0400029A RID: 666
			DWMWINDOWMAXIMIZEDCHANGE,
			/// <summary>
			/// Sent to request extended title bar information. A window receives this message through its WindowProc function.
			/// </summary>
			// Token: 0x0400029B RID: 667
			GETTITLEBARINFOEX = 831U,
			// Token: 0x0400029C RID: 668
			HANDHELDFIRST = 856U,
			// Token: 0x0400029D RID: 669
			HANDHELDLAST = 863U,
			// Token: 0x0400029E RID: 670
			AFXFIRST,
			// Token: 0x0400029F RID: 671
			AFXLAST = 895U,
			// Token: 0x040002A0 RID: 672
			PENWINFIRST,
			// Token: 0x040002A1 RID: 673
			PENWINLAST = 911U,
			/// <summary>
			/// The WM_APP constant is used by applications to help define private messages, usually of the form WM_APP+X, where X is an integer value. 
			/// </summary>
			// Token: 0x040002A2 RID: 674
			APP = 32768U,
			/// <summary>
			/// The WM_USER constant is used by applications to help define private messages for use by private window classes, usually of the form WM_USER+X, where X is an integer value. 
			/// </summary>
			// Token: 0x040002A3 RID: 675
			USER = 1024U,
			/// <summary>
			/// An application sends the WM_CPL_LAUNCH message to Windows Control Panel to request that a Control Panel application be started. 
			/// </summary>
			// Token: 0x040002A4 RID: 676
			CPL_LAUNCH = 5120U,
			/// <summary>
			/// The WM_CPL_LAUNCHED message is sent when a Control Panel application, started by the WM_CPL_LAUNCH message, has closed. The WM_CPL_LAUNCHED message is sent to the window identified by the wParam parameter of the WM_CPL_LAUNCH message that started the application. 
			/// </summary>
			// Token: 0x040002A5 RID: 677
			CPL_LAUNCHED,
			/// <summary>
			/// WM_SYSTIMER is a well-known yet still undocumented message. Windows uses WM_SYSTIMER for internal actions like scrolling.
			/// </summary>
			// Token: 0x040002A6 RID: 678
			SYSTIMER = 280U,
			/// <summary>
			/// The accessibility state has changed.
			/// </summary>
			// Token: 0x040002A7 RID: 679
			HSHELL_ACCESSIBILITYSTATE = 11U,
			/// <summary>
			/// The shell should activate its main window.
			/// </summary>
			// Token: 0x040002A8 RID: 680
			HSHELL_ACTIVATESHELLWINDOW = 3U,
			/// <summary>
			/// The user completed an input event (for example, pressed an application command button on the mouse or an application command key on the keyboard), and the application did not handle the WM_APPCOMMAND message generated by that input.
			/// If the Shell procedure handles the WM_COMMAND message, it should not call CallNextHookEx. See the Return Value section for more information.
			/// </summary>
			// Token: 0x040002A9 RID: 681
			HSHELL_APPCOMMAND = 12U,
			/// <summary>
			/// A window is being minimized or maximized. The system needs the coordinates of the minimized rectangle for the window.
			/// </summary>
			// Token: 0x040002AA RID: 682
			HSHELL_GETMINRECT = 5U,
			/// <summary>
			/// Keyboard language was changed or a new keyboard layout was loaded.
			/// </summary>
			// Token: 0x040002AB RID: 683
			HSHELL_LANGUAGE = 8U,
			/// <summary>
			/// The title of a window in the task bar has been redrawn.
			/// </summary>
			// Token: 0x040002AC RID: 684
			HSHELL_REDRAW = 6U,
			/// <summary>
			/// The user has selected the task list. A shell application that provides a task list should return TRUE to prevent Windows from starting its task list.
			/// </summary>
			// Token: 0x040002AD RID: 685
			HSHELL_TASKMAN,
			/// <summary>
			/// A top-level, unowned window has been created. The window exists when the system calls this hook.
			/// </summary>
			// Token: 0x040002AE RID: 686
			HSHELL_WINDOWCREATED = 1U,
			/// <summary>
			/// A top-level, unowned window is about to be destroyed. The window still exists when the system calls this hook.
			/// </summary>
			// Token: 0x040002AF RID: 687
			HSHELL_WINDOWDESTROYED,
			/// <summary>
			/// The activation has changed to a different top-level, unowned window.
			/// </summary>
			// Token: 0x040002B0 RID: 688
			HSHELL_WINDOWACTIVATED = 4U,
			/// <summary>
			/// A top-level window is being replaced. The window exists when the system calls this hook.
			/// </summary>
			// Token: 0x040002B1 RID: 689
			HSHELL_WINDOWREPLACED = 13U
		}

		// Token: 0x020000CD RID: 205
		public struct MSG
		{
			// Token: 0x040002B2 RID: 690
			public IntPtr hwnd;

			// Token: 0x040002B3 RID: 691
			public Win32.WM message;

			// Token: 0x040002B4 RID: 692
			public UIntPtr wParam;

			// Token: 0x040002B5 RID: 693
			public IntPtr lParam;

			// Token: 0x040002B6 RID: 694
			public int time;

			// Token: 0x040002B7 RID: 695
			public Win32.POINT pt;
		}

		// Token: 0x020000CE RID: 206
		[Flags]
		public enum PeekMessageParams : uint
		{
			// Token: 0x040002B9 RID: 697
			PM_NOREMOVE = 0U,
			// Token: 0x040002BA RID: 698
			PM_REMOVE = 1U,
			// Token: 0x040002BB RID: 699
			PM_NOYIELD = 2U,
			// Token: 0x040002BC RID: 700
			PM_QS_INPUT = 458752U,
			// Token: 0x040002BD RID: 701
			PM_QS_POSTMESSAGE = 9961472U,
			// Token: 0x040002BE RID: 702
			PM_QS_PAINT = 2097152U,
			// Token: 0x040002BF RID: 703
			PM_QS_SENDMESSAGE = 4194304U
		}

		// Token: 0x020000CF RID: 207
		[Flags]
		public enum QueueStatusFlags : uint
		{
			// Token: 0x040002C1 RID: 705
			QS_KEY = 1U,
			// Token: 0x040002C2 RID: 706
			QS_MOUSEMOVE = 2U,
			// Token: 0x040002C3 RID: 707
			QS_MOUSEBUTTON = 4U,
			// Token: 0x040002C4 RID: 708
			QS_MOUSE = 6U,
			// Token: 0x040002C5 RID: 709
			QS_INPUT = 7U,
			// Token: 0x040002C6 RID: 710
			QS_POSTMESSAGE = 8U,
			// Token: 0x040002C7 RID: 711
			QS_TIMER = 16U,
			// Token: 0x040002C8 RID: 712
			QS_PAINT = 32U,
			// Token: 0x040002C9 RID: 713
			QS_SENDMESSAGE = 64U,
			// Token: 0x040002CA RID: 714
			QS_HOTKEY = 128U,
			// Token: 0x040002CB RID: 715
			QS_REFRESH = 165U,
			// Token: 0x040002CC RID: 716
			QS_ALLEVENTS = 191U,
			// Token: 0x040002CD RID: 717
			QS_ALLINPUT = 255U,
			// Token: 0x040002CE RID: 718
			QS_ALLPOSTMESSAGE = 256U,
			// Token: 0x040002CF RID: 719
			QS_RAWINPUT = 1024U
		}
	}
}
