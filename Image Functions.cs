namespace Unicode_OCR
{
	public class character_image 
	{
		public void get_character_bounds()
		{			
			int i=1;
			int j=1;
			while(Convert.ToString (charimage1.GetPixel (i,j))=="Color [A=255, R=255, G=255, B=255]")
			{

				i++;
				if(i==100)
				{
					i=1;
					j++;
				}
				if(j>150)
					break;
			}
			if(j<150)			
				top=j;
			i=1;j=1;

			while(Convert.ToString (charimage1.GetPixel (i,j))=="Color [A=255, R=255, G=255, B=255]")
			{
				j++;
				if(j==150)
				{
					j=1;
					i++;
				}
				if(i>100)
					break;
			}
			if(i<100)			
				left=i;
			textBox1.Text = Convert.ToString (top ,10); 
			textBox2.Text = Convert.ToString (left ,10);
		}
	}
}