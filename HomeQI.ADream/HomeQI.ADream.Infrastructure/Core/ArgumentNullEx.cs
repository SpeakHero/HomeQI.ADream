
namespace System
{
    public class ArgumentNullEx : ArgumentException
    {
        //
        // 摘要:
        //     Initializes a new instance of the System.ArgumentException class.
        public ArgumentNullEx() : base()
        {
            LogerHelp.NLoger.Error("参数错误");
        }
        //
        // 摘要:
        //     Initializes a new instance of the System.ArgumentNullEx class with the
        //     name of the parameter that causes this exception.
        //
        // 参数:
        //   paramName:
        //     The name of the parameter that caused the exception.
        public ArgumentNullEx(string paramName) : base(paramName)
        {
            LogerHelp.NLoger.Error(paramName);
        }
        //
        // 摘要:
        //     Initializes a new instance of the System.ArgumentNullEx class with a specified
        //     error message and the exception that is the cause of this exception.
        //
        // 参数:
        //   message:
        //     The error message that explains the reason for this exception.
        //
        //   innerException:
        //     The exception that is the cause of the current exception, or a null reference
        //     (Nothing in Visual Basic) if no inner exception is specified.
        public ArgumentNullEx(string message, Exception innerException) : base(message, innerException)
        {
            LogerHelp.NLoger.Error(innerException, message);
        }
        //
        // 摘要:
        //     Initializes an instance of the System.ArgumentNullEx class with a specified
        //     error message and the name of the parameter that causes this exception.
        //
        // 参数:
        //   paramName:
        //     The name of the parameter that caused the exception.
        //
        //   message:
        //     A message that describes the error.
        public ArgumentNullEx(string paramName, string message) : base(paramName, message)
        {
            LogerHelp.NLoger.Error(message);
        }
    }

}
