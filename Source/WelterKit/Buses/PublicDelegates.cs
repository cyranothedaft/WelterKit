using System;

namespace WelterKit.Buses;

public delegate bool HandleGeneralMessageDelegate<in TMsg>(TMsg msg);
public delegate void HandleSpecificMessageDelegate<in TSubMsg>(TSubMsg msg);
