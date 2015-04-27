using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FTNoIRDOF
{
    class Program
    {
        struct THeadPoseData {
            double x, y, z, yaw, pitch, roll;
            long frame_number;

            // CONSTRUCTORS
	        public THeadPoseData(double _x, double _y, double _z, 
		        double _yaw, double _pitch, double _roll ) 
            {
                this.x = _x;
                this.y = _y;
                this.z = _z;
                this.yaw = _yaw;
                this.pitch = _pitch;
                this.roll = _roll;
                this.frame_number = 0;
            }

            public byte[] ToBytes()
            {
                Byte[] bytes = new Byte[Marshal.SizeOf(typeof(THeadPoseData))];
                GCHandle pinStructure = GCHandle.Alloc(this, GCHandleType.Pinned);
                try
                {
                    Marshal.Copy(pinStructure.AddrOfPinnedObject(), bytes, 0, bytes.Length);
                    return bytes;
                }
                finally
                {
                    pinStructure.Free();
                }
            } 
        }

        static void Main(string[] args)
        {
            UdpClient ftni_connection = new UdpClient();
            THeadPoseData rawHeadPose = new THeadPoseData(1,2,3,4,5,6);
            long frameCount = 0;

            Console.WriteLine("Connect to localhost:5550...");
            try
            {
                ftni_connection.Connect("localhost", 5550);
                Console.WriteLine("connected!");

                frameCount += 1;
                ftni_connection.Send(rawHeadPose.ToBytes(), rawHeadPose.ToBytes().Length);

                ftni_connection.Close();
                Console.WriteLine("Disconnected from localhost");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }
    }
}
