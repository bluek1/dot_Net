using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using bluekFramework.Struct;
using NReco.VideoConverter;
using NReco.VideoInfo;

namespace bluekFramework.Common {
    public class CommonUtil {

        /// <summary>
        /// 썸네일 작성
        /// </summary>
        /// <param name="AvFile">동영상 파일명</param>
        /// <param name="picFile">저장 이미지 파일명칭</param>
        /// <param name="PinCount">저장이미지 장수</param>
        public static List<String> CreateVideoThumbnail(String AvFile, String picFile, int PinCount) {
            List<String> rtn = new List<string>();
            FFMpegConverter ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            NRceoVideoInfo info = GetVedioInfo(AvFile);
            DateTime time = Convert.ToDateTime(info.PlayTime);
            double picPostion = time.TimeOfDay.TotalSeconds / (PinCount + 1); 
            for (int i = 1; i <= PinCount; i++) {
                String picName = String.Format(@"d:\{0}_{1}.jpg", picFile, i);
                double pos = picPostion * i;
                ffMpeg.GetVideoThumbnail(AvFile, picName, float.Parse(pos.ToString()));
                rtn.Add(picName);
            }
            return rtn;
        }

        /// <summary>
        /// 썸네일 작성
        /// </summary>
        /// <param name="AvFile">동영상 파일명</param>
        /// <param name="picFile">저장 이미지 파일명칭</param>
        /// <param name="PinCount">저장이미지 장수</param>
        public static List<MemoryStream> CreateVideoThumbnail(String AvFile, int PinCount) {
            List<MemoryStream> rtn = new List<MemoryStream>();
            FFMpegConverter ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            NRceoVideoInfo info = GetVedioInfo(AvFile);
            DateTime time = Convert.ToDateTime(info.PlayTime);
            double picPostion = time.TimeOfDay.TotalSeconds / (PinCount + 1);
            for (int i = 1; i <= PinCount; i++) {
                MemoryStream memStream = new MemoryStream();
                double pos = picPostion * i;
                ffMpeg.GetVideoThumbnail(AvFile, memStream, float.Parse(pos.ToString()) );
                rtn.Add(memStream);
            }
            return rtn;
        }
        /// <summary>
        /// 동영상 정보 취득
        /// </summary>
        /// <param name="filePath">동영상 파일명</param>
        /// <returns></returns>
        public static NRceoVideoInfo GetVedioInfo(String filePath) {
            NRceoVideoInfo rtn=new NRceoVideoInfo();
            try {
                var ffProbe = new FFProbe();
                var videoInfo = ffProbe.GetMediaInfo(filePath);
                //Console.WriteLine("Media information for: {0}", filePath);
                //Console.WriteLine("File format: {0}", videoInfo.FormatName);
                //Console.WriteLine("Duration: {0}", videoInfo.Duration);
                rtn.PlayTime = String.Format("{0}:{1}:{2}", videoInfo.Duration.Hours, videoInfo.Duration.Minutes, videoInfo.Duration.Seconds);
                foreach (var tag in videoInfo.FormatTags) {
                    //Console.WriteLine("\t{0}: {1}", tag.Key, tag.Value);
                }

                foreach (var stream in videoInfo.Streams) {
                    //Console.WriteLine("Stream {0} ({1})", stream.CodecName, stream.CodecType);
                    rtn.CodecName = stream.CodecName;
                    rtn.CodecType = stream.CodecType;
                    if (stream.CodecType == "video") {
                        //Console.WriteLine("\tFrame size: {0}x{1}", stream.Width, stream.Height);
                        rtn.Width = stream.Width.ToString();
                        rtn.Height = stream.Height.ToString();
                        rtn.FrameRate = String.Format("{0:0}", stream.FrameRate);
                    }
                    foreach (var tag in stream.Tags) {
                        //  Console.WriteLine("\t{0}: {1}", tag.Key, tag.Value);
                    }
                }
            } catch (Exception e) {
                rtn.Width = "";
                rtn.Height = "";
                rtn.FrameRate = "";
            }
            return rtn;
        }
        /// <summary>
        /// MD5 취즉
        /// </summary>
        /// <param name="FilePath">대상 파일명</param>
        /// <returns></returns>
        public static string ComputeMD5Hash(string FilePath) {
            return ComputeHash(FilePath, new MD5CryptoServiceProvider());
        }

        /// <summary>
        ///  해쉬 취즉
        /// </summary>
        /// <param name="FilePath">대상 파일명</param>
        /// <param name="Algorithm">알고리즘</param>
        /// <returns></returns>
        private static string ComputeHash(string FilePath, HashAlgorithm Algorithm) {
            FileStream FileStream = File.OpenRead(FilePath);
            try {
                byte[] HashResult = Algorithm.ComputeHash(FileStream);
                string ResultString = BitConverter.ToString(HashResult).Replace("-", "");
                return ResultString;
            } finally {
                FileStream.Close();
            }
        }
        /// <summary>
        /// 핑 신호
        /// </summary>
        /// <param name="ip">아이피</param>
        /// <param name="port">포트</param>
        /// <returns></returns>
        public static bool Ping(string ip, int port) {
            try {
                //IP Address 할당 
                IPAddress ipAddress = IPAddress.Parse(ip);

                //TCP Client 선언
                TcpClient tcpClient = new TcpClient(AddressFamily.InterNetwork);

                Ping pingSender = new Ping();
                PingOptions options = new PingOptions();

                // Use the default Ttl value which is 128,
                // but change the fragmentation behavior.
                options.DontFragment = true;

                // Create a buffer of 32 bytes of data to be transmitted.
                string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                PingReply reply = pingSender.Send(ipAddress, timeout, buffer, options);

                if (reply.Status == IPStatus.Success) {
                    // Ping 성공시 Connect 연결 시도
                    tcpClient.NoDelay = true;
                    tcpClient.Connect(ipAddress, port);

                    NetworkStream ntwStream = tcpClient.GetStream();
                } else {
                    // Ping 실패시 강제 Exception
                    throw new Exception();
                }

                return true;
            } catch (Exception ex) {
                //MessageBox.Show("Connect Fail... : " + ex);
                return false;
            }
        }
        /// <summary>
        /// WOL신호 전소
        /// </summary>
        /// <param name="mac">맥어드레스</param>
        public static void WakeUp(byte[] mac) {
            UdpClient client = new UdpClient();
            client.Connect(IPAddress.Broadcast, 40000);

            byte[] packet = new byte[17 * 6];

            for (int i = 0; i < 6; i++) {
                packet[i] = 0xFF;
            }

            for (int i = 1; i <= 16; i++) {
                for (int j = 0; j < 6; j++) {
                    packet[i * 6 + j] = mac[j];
                }
            }
            client.Send(packet, packet.Length);
        }
        /// <summary>
        /// 파일의 사용중인지 확인
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static bool IsFileLocked(String path) {
            FileStream stream = null;
            FileInfo filepath = new FileInfo(path);
            try {
                stream = filepath.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            } catch (IOException) {
                return true;
            } finally {
                if (stream != null)
                    stream.Close();
            }
            return false;
        }

        public static void FileCopy(String source_file_nm, String target_file_nm, Action<int, int> progBarEvent) {
            byte[] buf = new byte[1024*512]; 
            String target_path = Path.GetDirectoryName(target_file_nm);
            if (!Directory.Exists(target_path)) {
                try {
                    Directory.CreateDirectory(target_path);
                }catch(Exception e) {
                    throw new IOException("PATH");
                }

            }
            FileStream strIn = new FileStream(source_file_nm, FileMode.Open);
            FileStream strOut = new FileStream(target_file_nm, FileMode.OpenOrCreate);

            try {
                while (strIn.Position < strIn.Length) {
                    int len = strIn.Read(buf, 0, buf.Length);
                    strOut.Write(buf, 0, len);

                    if (progBarEvent != null) {
                        progBarEvent(Convert.ToInt32(strIn.Position / (1024 * 1024)), Convert.ToInt32(strIn.Length / (1024 * 1024)));
                    }
                }
            } catch (Exception ex) {
                throw new IOException("COPY");
            } finally {
                strOut.Flush();
                strIn.Close();
                strOut.Close();
            }
        }

    }

    


}
