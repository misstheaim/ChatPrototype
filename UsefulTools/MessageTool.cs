using System.Text;

namespace ChatPrototype.UsefulTools
{
    public class MessageTool
    {
        int BufferCountLimit = 20;
        int BufferCount = 0;
        Encoding unicode = Encoding.Unicode;
        List<string> MessageaStringBuffer;
        List<byte[]> MessagebByteBuffer;

        public MessageTool()
        {
            MessageaStringBuffer = new List<string>(BufferCountLimit);
            MessagebByteBuffer = new List<byte[]>(BufferCountLimit);
        }
        public byte[] AddByteArray(byte[] buf)
        {

            int lengthOfMessage = 0;
            foreach (byte b in buf)
            {
                if (b > 0) lengthOfMessage += 1;
                else break;
            }

            //Useless block, for now
            char[] chars = new char[unicode.GetCharCount(buf, 0, buf.Length)];
            unicode.GetChars(buf, 0, buf.Length, chars, 0);
            string message = new string(chars);
            //-----------------------

            if (BufferCount == BufferCountLimit)
            {
                MessageaStringBuffer.RemoveAt(0);
                MessagebByteBuffer.RemoveAt(0);
            }

            byte[] messageByte = new byte[lengthOfMessage];
            Array.Copy(buf, 0, messageByte, 0, lengthOfMessage);
            MessageaStringBuffer.Add(message);
            MessagebByteBuffer.Add(messageByte);

            return messageByte;
        }
        public List<byte[]> GetBuffer()
        {
            return MessagebByteBuffer;
        }
        
    }
}
