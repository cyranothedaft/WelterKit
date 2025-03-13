using WelterKit.Std.Functional;

namespace WelterKit.Buses;

public interface IBusWriter<in TMsg> {
   void Write(TMsg message);
   void Stop();
}
