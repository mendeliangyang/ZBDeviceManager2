using System;

namespace ZB.DriveCommon
{
    public class GuidHelper
    {
        public static string GetStringGuid()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
