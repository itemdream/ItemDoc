һ��Gifͼ���ļ������м����ļ����кϳɵģ���˴�������ļ���ʱ�򣬲�����Jpeg����Bmp�ļ���������
��Ҫ��Gif�ļ����֡����ʽ��Ȼ���ÿһ֡���д�����������ٺϳ�Gif��
  
AnimatedGifEncoder ���ʵ���ѷ�װ�� Gif.Components.dll ��,
�����ļ���Ҫ����Gif.Components.dll, ����������:
using System; 
using System.Drawing; 
using System.Drawing.Imaging; 
using ItemDoc.Framework.Imaging.Gif;
  

	    String[] imageFilePaths = new String[] { "c:\\01.png", "c:\\02.png", "c:\\03.png" };
            String outputFilePath = "c:\\test.gif";
            AnimatedGifEncoder e1 = new AnimatedGifEncoder();
            e1.Start(outputFilePath);
            e1.SetDelay(500);    // �ӳټ��
            e1.SetRepeat(0);  //-1:��ѭ��,0:����ѭ�� ����   
            for (int i = 0, count = imageFilePaths.Length; i < count; i++)
            {
                e1.AddFrame(Image.FromFile(imageFilePaths[i]));
            }
            e1.Finish();
            /////////////////////////
            string outputPath = "c:\\";
            GifDecoder gifDecoder = new GifDecoder();
            gifDecoder.Read("c:\\test.gif"); 
            for (int i = 0, count = gifDecoder.GetFrameCount(); i < count; i++)
            {
                Image frame = gifDecoder.GetFrame(i); // frame i  
                frame.Save(outputPath + Guid.NewGuid().ToString() + ".png", ImageFormat.Png);
            }

��������PNGͼƬ,�ŵ�C�̸�Ŀ¼����.