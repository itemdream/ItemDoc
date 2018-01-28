using System;
using System.Text;

namespace ItemDoc.Framework.Helper
{
    /// <summary>
    /// ������������ת��������ת��������ת����ص���
    /// </summary>    
    public class ConvertHelper
    {
        #region �����ת��
        /// <summary> 
        /// ���ֽ��ת������Һ��ִ�д��� 
        /// </summary> 
        /// <param name="num">���ֽ��</param> 
        /// <returns>���ش�д���ֽ��</returns> 
        public static string ChangeMoneyToCn(decimal num)
        {
            string str1 = "��Ҽ��������½��ƾ�";                  //0-9����Ӧ�ĺ��� 
            string str2 = "��Ǫ��ʰ��Ǫ��ʰ��Ǫ��ʰԪ�Ƿ�";        //����λ����Ӧ�ĺ��� 
            string OldNum = "";    //��ԭnumֵ��ȡ����ֵ 
            string NumMoney = "";      //  ���ֵ��ַ�����ʽ 
            string CapitalMoney = "";  //����Ҵ�д�����ʽ 
            int i;    //ѭ������ 
            int j;    //num��ֵ����100���ַ������� 
            string ch1 = "";    //���ֵĺ������ 
            string ch2 = "";    //����λ�ĺ��ֶ��� 
            int nzero = 0;  //����������������ֵ�Ǽ��� 
            int temp;            //��ԭnumֵ��ȡ����ֵ 

            num = Math.Round(Math.Abs(num), 2);    //��numȡ����ֵ����������ȡ2λС�� 
            NumMoney = ((long)(num * 100)).ToString();        //��num��100��ת�����ַ�����ʽ 
            j = NumMoney.Length;                          //�ҳ����λ 
            if (j > 16)
                return "������λ��̫�����";
            str2 = str2.Substring(16 - j);   //ȡ����Ӧλ����str2��ֵ���磺200.55,jΪ5����str2=��ʰԪ�Ƿ� 

            //ѭ��ȡ��ÿһλ��Ҫת����ֵ 
            for (i = 0; i < j; i++)
            {
                OldNum = NumMoney.Substring(i, 1);          //ȡ����ת����ĳһλ��ֵ 
                temp = Convert.ToInt32(OldNum);      //ת��Ϊ���� 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //����ȡλ����ΪԪ�����ڡ������ϵ�����ʱ 
                    if (OldNum == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (OldNum != "0" && nzero != 0)
                        {
                            ch1 = "��" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //��λ�����ڣ��ڣ���Ԫλ�ȹؼ�λ 
                    if (OldNum != "0" && nzero != 0)
                    {
                        ch1 = "��" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (OldNum != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (OldNum == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                {
                    //�����λ����λ��Ԫλ�������д�� 
                    ch2 = str2.Substring(i, 1);
                }
                CapitalMoney = CapitalMoney + ch1 + ch2;

                if (i == j - 1 && OldNum == "0")
                {
                    //���һλ���֣�Ϊ0ʱ�����ϡ����� 
                    CapitalMoney = CapitalMoney + '��';
                }

            }
            if (num == 0)
            {
                CapitalMoney = "��Ԫ��";
            }
            return CapitalMoney;
        }


 
        #endregion


        
        /// <summary>
        /// ָ���ַ����Ĺ̶����ȣ�����ַ���С�ڹ̶����ȣ�
        /// �����ַ�����ǰ�油���㣬�����õĹ̶��������Ϊ9λ
        /// </summary>
        /// <param name="text">ԭʼ�ַ���</param>
        /// <param name="limitedLength">�ַ����Ĺ̶�����</param>
        public static string RepairZero(string text, int limitedLength)
        {
            //����0���ַ���
            string temp = "";

            //����0
            for (int i = 0; i < limitedLength - text.Length; i++)
            {
                temp += "0";
            }

            //����text
            temp += text;

            //���ز���0���ַ���
            return temp;
        }
        

        #region ����������ת��
        /// <summary>
        /// ʵ�ָ����������ת����ConvertBase("15",10,16)��ʾ��ʮ������15ת��Ϊ16���Ƶ�����
        /// </summary>
        /// <param name="value">Ҫת����ֵ,��ԭֵ</param>
        /// <param name="from">ԭֵ�Ľ���,ֻ����2,8,10,16�ĸ�ֵ��</param>
        /// <param name="to">Ҫת������Ŀ����ƣ�ֻ����2,8,10,16�ĸ�ֵ��</param>
        public static string ConvertBase(string value, int from, int to)
        {
            try
            {
                int intValue = Convert.ToInt32(value, from);  //��ת��10����
                string result = Convert.ToString(intValue, to);  //��ת��Ŀ�����
                if (to == 2)
                {
                    int resultLength = result.Length;  //��ȡ�����Ƶĳ���
                    switch (resultLength)
                    {
                        case 7:
                            result = "0" + result;
                            break;
                        case 6:
                            result = "00" + result;
                            break;
                        case 5:
                            result = "000" + result;
                            break;
                        case 4:
                            result = "0000" + result;
                            break;
                        case 3:
                            result = "00000" + result;
                            break;
                    }
                }
                return result;
            }
            catch
            {

                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return "0";
            }
        }
        #endregion

        #region ʹ��ָ���ַ�����stringת����byte[]
        /// <summary>
        /// ʹ��ָ���ַ�����stringת����byte[]
        /// </summary>
        /// <param name="text">Ҫת�����ַ���</param>
        /// <param name="encoding">�ַ�����</param>
        public static byte[] StringToBytes(string text, Encoding encoding)
        {
            return encoding.GetBytes(text);
        }
        #endregion

        #region ʹ��ָ���ַ�����byte[]ת����string
        /// <summary>
        /// ʹ��ָ���ַ�����byte[]ת����string
        /// </summary>
        /// <param name="bytes">Ҫת�����ֽ�����</param>
        /// <param name="encoding">�ַ�����</param>
        public static string BytesToString(byte[] bytes, Encoding encoding)
        {
            return encoding.GetString(bytes);
        }
        #endregion

        #region ��byte[]ת����int
        /// <summary>
        /// ��byte[]ת����int
        /// </summary>
        /// <param name="data">��Ҫת����������byte����</param>
        public static int BytesToInt32(byte[] data)
        {
            //���������ֽ����鳤��С��4,�򷵻�0
            if (data.Length < 4)
            {
                return 0;
            }

            //����Ҫ���ص�����
            int num = 0;

            //���������ֽ����鳤�ȴ���4,��Ҫ���д���
            if (data.Length >= 4)
            {
                //����һ����ʱ������
                byte[] tempBuffer = new byte[4];

                //��������ֽ������ǰ4���ֽڸ��Ƶ���ʱ������
                Buffer.BlockCopy(data, 0, tempBuffer, 0, 4);

                //����ʱ��������ֵת����������������num
                num = BitConverter.ToInt32(tempBuffer, 0);
            }

            //��������
            return num;
        }
        #endregion


    }
}
