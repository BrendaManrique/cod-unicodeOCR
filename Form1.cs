using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Text;
using System.IO;
using System.Threading;



namespace Unicode_OCR
{	
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
		private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
		private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button button14;	
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label label24;
		private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label29;	
		private System.Windows.Forms.RichTextBox richTextBox1;	
		private System.ComponentModel.IContainer components=null;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;

		public
															//LAST WORKING VALUES
		const int number_of_layers=3;						//3
		const int number_of_input_nodes=150;				//150
		const int number_of_output_nodes=16;	     		//16
		const int maximum_layers=250;					    //250
		const int maximum_number_of_sets=100;				//100
		int number_of_input_sets;					        
		int epochs =300;	   							    //300 & 600(For Latin Arial)
		const float error_threshold=0.0002F;				//0.0002F
		

		float learning_rate=150F;							//150F
		float slope=0.014F;									//0.014F	
		int weight_bias=30;									//30

		int[] layers=new int [number_of_layers];
		float[] current_input=new float [number_of_input_nodes];
		float[,] input_set=new float [number_of_input_nodes,maximum_number_of_sets];
		int[] desired_output=new int [number_of_output_nodes];
		int[,] desired_output_set=new int [number_of_output_nodes,maximum_number_of_sets];
		float[,] node_output=new float [number_of_layers,maximum_layers];
		float[,,] weight=new float [number_of_layers,maximum_layers,maximum_layers];
		float[,] error=new float [number_of_layers,maximum_layers];		
		int[] output_bit=new int [number_of_output_nodes];	
		int[] desired_output_bit = new int [number_of_output_nodes];

		const int rec_width = 5;
		const int rec_height = 5;
		const int x_org=500;
		const int y_org=50;

		const int matrix_width=20;
		const int matrix_height=30;
		int image_start_pixel_x=0;
		int image_start_pixel_y=0;
		int[] line_top=new int [20];
		int[] line_bottom=new int [20];
		int current_line=0;
		int number_of_lines=0;
		bool line_present=true;
		bool character_valid=true;
		bool character_present=true;
		bool trainer_thread_created=false;

		string image_file_name;
		string image_file_path;
		string character_trainer_set_file_name;
		string character_trainer_set_file_path;
		string network_file_name;		
		string trainer_string;		
		string output_string;
		System.IO.StreamReader  image_file_stream;
		System.IO.StreamReader  character_trainer_set_file_stream;
		System.IO.StreamWriter  network_save_file_stream;
		System.IO.StreamReader  network_load_file_stream;
		
		
		Random rnd = new Random();
		Thread trainer_thread;	
		UnicodeEncoding unicode = new UnicodeEncoding();
			
		
		Color[,] ann_input_pixel = new Color[20,30];
		Color[,] character_image_pixel = new Color[600,800];
		int[,] ann_input_value=new int [20,30];
		
		int[] sample_pixel_x=new int [20];
		int[] sample_pixel_y=new int [30];
					
		int input_image_height;
		int input_image_width;
		int top,bottom,left,right;	
		int prev_right=20;
		int character_height;
		int character_width;

        Bitmap input_image;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button button1;							
		Bitmap character_image;			
				
		public Form1()
		{			
			InitializeComponent();			
			// TODO: Add any constructor code after InitializeComponent call			
		}
	
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.Configuration.AppSettingsReader configurationAppSettings = new System.Configuration.AppSettingsReader();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button3 = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.button14 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(6, 44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(336, 150);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 198);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 24);
            this.button2.TabIndex = 5;
            this.button2.Text = "Cargar";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox2.Location = new System.Drawing.Point(353, 44);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(100, 150);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.AddExtension = false;
            this.openFileDialog1.DefaultExt = ((string)(configurationAppSettings.GetValue("openFileDialog1.DefaultExt", typeof(string))));
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(104, 198);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 24);
            this.button3.TabIndex = 9;
            this.button3.Text = "Caracter";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label22
            // 
            this.label22.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label22.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label22.Location = new System.Drawing.Point(365, 340);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(64, 16);
            this.label22.TabIndex = 16;
            this.label22.Text = "No existe";
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.Black;
            this.label23.Location = new System.Drawing.Point(298, 343);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(48, 16);
            this.label23.TabIndex = 15;
            this.label23.Text = "% Error :";
            // 
            // label16
            // 
            this.label16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label16.Location = new System.Drawing.Point(365, 317);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(64, 16);
            this.label16.TabIndex = 6;
            this.label16.Text = "0";
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(298, 317);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(56, 16);
            this.label14.TabIndex = 4;
            this.label14.Text = "Contador:";
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button4.Location = new System.Drawing.Point(445, 324);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(63, 35);
            this.button4.TabIndex = 1;
            this.button4.Text = "Inicio";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(18, 280);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(256, 88);
            this.richTextBox1.TabIndex = 38;
            this.richTextBox1.Text = "";
            // 
            // groupBox6
            // 
            this.groupBox6.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.button2);
            this.groupBox6.Controls.Add(this.button3);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.label29);
            this.groupBox6.Controls.Add(this.label28);
            this.groupBox6.Controls.Add(this.label24);
            this.groupBox6.Controls.Add(this.label19);
            this.groupBox6.Controls.Add(this.pictureBox2);
            this.groupBox6.Controls.Add(this.button14);
            this.groupBox6.Controls.Add(this.pictureBox1);
            this.groupBox6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.groupBox6.Location = new System.Drawing.Point(12, 12);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(616, 242);
            this.groupBox6.TabIndex = 28;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "OCR";
            this.groupBox6.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox6_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(481, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 26;
            this.label3.Text = "Cuadrícula";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(350, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 26;
            this.label2.Text = "Letra:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(6, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 15);
            this.label1.TabIndex = 26;
            this.label1.Text = "Imágen";
            // 
            // label29
            // 
            this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.Color.Black;
            this.label29.Location = new System.Drawing.Point(480, 190);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(16, 16);
            this.label29.TabIndex = 25;
            this.label29.Text = "29";
            // 
            // label28
            // 
            this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.Black;
            this.label28.Location = new System.Drawing.Point(486, 48);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(8, 16);
            this.label28.TabIndex = 24;
            this.label28.Text = "0";
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.Black;
            this.label24.Location = new System.Drawing.Point(584, 202);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(16, 16);
            this.label24.TabIndex = 23;
            this.label24.Text = "19";
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.Location = new System.Drawing.Point(494, 202);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(16, 16);
            this.label19.TabIndex = 22;
            this.label19.Text = "0";
            // 
            // button14
            // 
            this.button14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button14.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button14.Location = new System.Drawing.Point(353, 202);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(96, 24);
            this.button14.TabIndex = 0;
            this.button14.Text = "Start";
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "ann";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(289, 280);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(176, 24);
            this.button1.TabIndex = 0;
            this.button1.Text = "Entrenar";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(754, 407);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label14);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "OCR_Bren";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());			
		}		
		private void button2_Click(object sender, System.EventArgs e)
		{									
			load_image();
		}
		private void button3_Click(object sender, System.EventArgs e)
		{
			detect_next_character();			
		}
		private void button14_Click(object sender, System.EventArgs e)
        {
            load_network();
			output_string="";
			current_line=0;
			while(character_present)
				detect_next_character();		
		}	
		private void button1_Click(object sender, System.EventArgs e)
		{	
			load_character_trainer_set();			
		}
		private void button7_Click(object sender, System.EventArgs e)
		{
			save_network();
		}
		private void button15_Click(object sender, System.EventArgs e)
		{
			
		}
		private void button13_Click(object sender, System.EventArgs e)
		{
			save_output();
		}

	////////////////////////////////////////////////////////////////////////////////////
	////                    IMAGE ANALYSIS AND MANIPULATION CODE                   /////
	////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////
	
		
		public void load_character_trainer_set()
		{	
			string line;
			openFileDialog1.Filter = "Character Trainer Set (*.cts)|*.cts" ;
			if(openFileDialog1.ShowDialog() == DialogResult.OK)
			{				
				character_trainer_set_file_stream = new	System.IO.StreamReader(openFileDialog1.FileName);				
				trainer_string="";
				while((line=character_trainer_set_file_stream.ReadLine ())!=null)
					trainer_string=trainer_string+line;																	
				number_of_input_sets=trainer_string.Length ;
				character_trainer_set_file_name=Path.GetFileNameWithoutExtension(openFileDialog1.FileName );
				character_trainer_set_file_path=Path.GetDirectoryName(openFileDialog1.FileName);
				character_trainer_set_file_stream.Close();

				image_file_name=character_trainer_set_file_path + "\\"+ character_trainer_set_file_name + ".bmp";
				image_file_stream =new System.IO.StreamReader(image_file_name);
				input_image=new Bitmap (image_file_name);
				//pictureBox1.Image =input_image;	
				input_image_height=input_image.Height ;
				input_image_width=input_image.Width ;
				//if(input_image_width>pictureBox1.Width ) 
					//pictureBox1.SizeMode= PictureBoxSizeMode.StretchImage;
				//else
					//pictureBox1.SizeMode= PictureBoxSizeMode.Normal;
				right=1;
				image_start_pixel_x=0;
				image_start_pixel_y=0;
				identify_lines();
				current_line=0;
				character_present=true;
				character_valid=true;
				output_string="";				
				
			}
		}
		public void load_image()
		{
			openFileDialog1.Filter = "Bitmap Image (*.bmp)|*.bmp" ;
			if(openFileDialog1.ShowDialog() == DialogResult.OK)
			{				
				System.IO.StreamReader image_file_stream = new	System.IO.StreamReader(openFileDialog1.FileName);				
				input_image=new Bitmap (openFileDialog1.FileName);		
				pictureBox1.Image =input_image;								
				image_file_name=Path.GetFileNameWithoutExtension(openFileDialog1.FileName );
				image_file_path=Path.GetDirectoryName(openFileDialog1.FileName);				
				image_file_stream.Close();
				input_image_height=input_image.Height ;
				input_image_width=input_image.Width ;
				if(input_image_width>pictureBox1.Width ) 
					pictureBox1.SizeMode= PictureBoxSizeMode.StretchImage;
				else
					pictureBox1.SizeMode= PictureBoxSizeMode.Normal;
				right=1;
				image_start_pixel_x=0;
				image_start_pixel_y=0;
				identify_lines();
				current_line=0;
				character_present=true;
				character_valid=true;
				output_string="";
				
			}
		}
		public void detect_next_character()
		{			
			number_of_input_sets=1;
			get_next_character();
			if(character_present)
			{
				for(int i=0;i<10;i++)
					for(int j=0;j<15;j++)
						input_set[i*15+j,0]=ann_input_value[i*2+1,j*2+1];
				get_inputs(0);
				calculate_outputs();
				for(int i=0;i<number_of_output_nodes;i++)
				{
					output_bit[i]=threshold(node_output[number_of_layers-1,i]);
					}
				char character=unicode_to_character();
				output_string=output_string+character.ToString ();
				string hexadecimal=binary_to_hex();
				richTextBox1.Text =output_string;			
				richTextBox1.Update ();
			}
		}
		public void get_next_character()
		{			
			image_start_pixel_x=right+2;
			image_start_pixel_y=line_top[current_line];
			analyze_image();
		}
		public void analyze_image()
		{			
			int analyzed_line=current_line;
			
			
			get_character_bounds();
            if (character_present)
            {
                map_character_image_pixel_matrix();
                create_character_image();
                map_ann_input_matrix();
            }
            else
                button14.Enabled = true;
				/*MessageBox.Show ("Completado", "OCR_Bren", 
					MessageBoxButtons.OK , MessageBoxIcon.Exclamation);
			*/
		}
		private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{								
		}
		public void identify_lines()
		{			
			int y=image_start_pixel_y;
			int x=image_start_pixel_x;
			bool no_black_pixel;
			int line_number=0;
			line_present=true;
			while(line_present)
			{				
				x=image_start_pixel_x;
				while(Convert.ToString (input_image.GetPixel (x,y))=="Color [A=255, R=255, G=255, B=255]")
				{
					x++;
					if(x==input_image_width)
					{
						x=image_start_pixel_x;
						y++;
					}
					if(y>=input_image_height)
					{
						line_present=false;
						break;
					}
				}
				if(line_present)
				{
					line_top[line_number]=y;
					no_black_pixel=false;
					while(no_black_pixel==false)
					{				
						y++;
						no_black_pixel=true;
						for(x=image_start_pixel_x;x<input_image_width;x++)				
							if((Convert.ToString (input_image.GetPixel (x,y))=="Color [A=255, R=0, G=0, B=0]"))					
								no_black_pixel=false;														
					}			
					line_bottom[line_number]=y-1;
					line_number++;
				}
			}
			number_of_lines=line_number;			
		}
		public void get_character_bounds()
		{			
			int x=image_start_pixel_x;
			int y=image_start_pixel_y;			
			bool no_black_pixel=false;
			if(y<=input_image_height && x<=input_image_width)
			{
				while(Convert.ToString (input_image.GetPixel (x,y))=="Color [A=255, R=255, G=255, B=255]")
				{
					x++;
					if(x==input_image_width)
					{
						x=image_start_pixel_x;
						y++;
					}
					if(y>=line_bottom[current_line])
					{
						character_present=false;
						break;
					}
				}
				if(character_present)
				{
					top=y;
			
					x=image_start_pixel_x; y=image_start_pixel_y;	
					while(Convert.ToString (input_image.GetPixel (x,y))=="Color [A=255, R=255, G=255, B=255]")
					{
						y++;
						if(y==line_bottom[current_line])
						{
							y=image_start_pixel_y;
							x++;
						}
						if(x>input_image_width)
							break;
					}
					if(x<input_image_width)			
						left=x;

					no_black_pixel=true;
					y=line_bottom[current_line]+2;
					while(no_black_pixel==true)
					{				
						y--;						
						for(x=image_start_pixel_x;x<input_image_width;x++)				
							if((Convert.ToString (input_image.GetPixel (x,y))=="Color [A=255, R=0, G=0, B=0]"))					
								no_black_pixel=false;														
					}			
					bottom=y;

					no_black_pixel=false;
					x=left+10;
					while(no_black_pixel==false)
					{
						x++;
						no_black_pixel=true;
						for(y=image_start_pixel_y;y<line_bottom[current_line];y++)				
							if((Convert.ToString (input_image.GetPixel (x,y))=="Color [A=255, R=0, G=0, B=0]"))					
								no_black_pixel=false;														
					}		
					right=x-1;
					top=confirm_top();
					bottom=confirm_bottom();					
				
					character_height=bottom-top+1;
					character_width=right-left+1;
					confirm_dimensions();
					if(left-prev_right>=20)
						output_string=output_string+" ";

					prev_right=right;

				}
				else if(current_line<number_of_lines-1)
				{					
					current_line++;
					image_start_pixel_y=line_top[current_line];
					image_start_pixel_x=0;
					prev_right=20;
					output_string=output_string + "\n";
					character_present=true;
					get_character_bounds();																									
				}
			}
			else
				character_present=false;
		}
		public int confirm_top()
		{
			int local_top=top;
			for(int j=top;j<=bottom;j++)
				for(int i=left;i<=right;i++)
					if(Convert.ToString (input_image.GetPixel (i,j))=="Color [A=255, R=0, G=0, B=0]")
					{
						local_top=j;
						return local_top;
					}
			return local_top;
		}
		public int confirm_bottom()
		{
			int local_bottom=bottom;
			for(int j=bottom;j>=0;j--)
				for(int i=left;i<=right;i++)
					if(Convert.ToString (input_image.GetPixel (i,j))!="Color [A=255, R=255, G=255, B=255]")
					{
						local_bottom=j;
						return local_bottom;
					}			
			return local_bottom;
		}
		public void confirm_dimensions()
		{
			if(character_width<20)
			{
				left=left-5; right=right+5;				
			}
			if(character_height<30)
			{
				top=top-15; bottom=bottom+15;				
			}
			character_height=bottom-top+1;
			character_width=right-left+1;
		}
		public void pick_sampling_pixels()
		{			
			int step=(int)(character_height/matrix_height);
			if(step<1) step=1;

			sample_pixel_y[0]=0;			
			sample_pixel_y[29]=character_height-1;
			sample_pixel_y[19]=(int)(2*sample_pixel_y[29]/3);			
			sample_pixel_y[9]=(int)(sample_pixel_y[29]/3);
			
			sample_pixel_y[4]=(int)(sample_pixel_y[9]/2);
			sample_pixel_y[5]=sample_pixel_y[4]+step;
			sample_pixel_y[2]=(int)(sample_pixel_y[4]/2);
			sample_pixel_y[3]=sample_pixel_y[2]+step;
			sample_pixel_y[1]=sample_pixel_y[0]+step;
			sample_pixel_y[6]=sample_pixel_y[1]+sample_pixel_y[5];
			sample_pixel_y[7]=sample_pixel_y[2]+sample_pixel_y[5];
			sample_pixel_y[8]=sample_pixel_y[3]+sample_pixel_y[5];
			for(int i=10;i<19;i++)
				sample_pixel_y[i]=sample_pixel_y[i-10]+sample_pixel_y[9];
			for(int i=20;i<29;i++)
				sample_pixel_y[i]=sample_pixel_y[i-20]+sample_pixel_y[19];

			step=(int)(character_width/matrix_width);
			if(step<1) step=1;
			
			sample_pixel_x[0]=0;			
			sample_pixel_x[19]=character_width-1;			
			sample_pixel_x[9]=(int)(sample_pixel_x[19]/2);						
			
			sample_pixel_x[4]=(int)(sample_pixel_x[9]/2);
			sample_pixel_x[5]=sample_pixel_x[4]+step;
			sample_pixel_x[2]=(int)(sample_pixel_x[4]/2);
			sample_pixel_x[3]=sample_pixel_x[2]+step;
			sample_pixel_x[1]=sample_pixel_x[0]+step;
			sample_pixel_x[6]=sample_pixel_x[1]+sample_pixel_x[5];
			sample_pixel_x[7]=sample_pixel_x[2]+sample_pixel_x[5];
			sample_pixel_x[8]=sample_pixel_x[3]+sample_pixel_x[5];
			for(int i=10;i<19;i++)
				sample_pixel_x[i]=sample_pixel_x[i-10]+sample_pixel_x[9];			

			
		}
		public void map_character_image_pixel_matrix()
		{												
			for(int j=0;j<character_height;j++)			
				for(int i=0;i<character_width;i++)				
					character_image_pixel[i,j]=input_image.GetPixel (i+left,j+top);																
		}
		public void create_character_image()
		{
			character_image = new System.Drawing.Bitmap(character_width, character_height);
			for(int j=0;j<character_height;j++)
				for(int i=0;i<character_width;i++)
					character_image.SetPixel (i,j,character_image_pixel[i,j]);
			pictureBox2.Image =character_image;
			pictureBox2.Update ();	
		}
		public void map_ann_input_matrix()
		{			
			pick_sampling_pixels();			
			for( int j = 0; j < matrix_height; j++)			
				for(int i= 0; i < matrix_width; i++ )	
				{														
					ann_input_pixel[i,j]=character_image.GetPixel(sample_pixel_x[i],sample_pixel_y[j]);	
					if(ann_input_pixel[i,j].ToString()=="Color [A=255, R=0, G=0, B=0]")
						ann_input_value[i,j]=1;
					else
						ann_input_value[i,j]=0;
				}
			groupBox6.Invalidate ();
			groupBox6.Update ();
		}
		private void groupBox6_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			SolidBrush blueBrush = new SolidBrush(Color.Blue);
			Pen blackpen=new Pen(Color.Black, 1);
			for(int j=0;j<matrix_height;j++)
				for(int i=0;i<matrix_width;i++)
				{					
					e.Graphics.DrawRectangle(blackpen, (x_org+rec_width*i),(y_org+rec_height*j),(rec_width),(rec_height));
					if(ann_input_value[i,j]==1)
						e.Graphics.FillRectangle(blueBrush, x_org+rec_width*i,y_org+rec_height*j,rec_width,rec_height);			
				}			
		}
		private void button6_Click(object sender, System.EventArgs e)
		{
			if(trainer_thread_created==true)
			{
				if(trainer_thread.ThreadState ==System.Threading.ThreadState.Suspended)
					trainer_thread.Resume ();			
				trainer_thread.Abort();		
			}
			Application.Exit ();
		}
		private void button5_Click(object sender, System.EventArgs e)
		{
			MessageBox.Show ("", "OCR_bms", 
				MessageBoxButtons.OK , MessageBoxIcon.Information);
		}


///
	////////////////////////////////////////////////////////////////////////////////////
	////       MULTI-LAYER PERCEPTRON NEURAL NETWORK IMPLEMENTATION                /////
	////////////////////////////////////////////////////////////////////////////////////
	////////////////////////////////////////////////////////////////////////////////////

		private void button4_Click(object sender, System.EventArgs e)
		{
            metodo();
        }
        private void metodo()
        {
            
	
            reset_controls();
			form_network();
			initialize_weights();			
			form_input_set();
			form_desired_output_set();
			right=1;
			trainer_thread = new Thread(new ThreadStart(train_network));
			trainer_thread_created=true;
            button14.Enabled = true;
			trainer_thread.Start();
            	
		}				
		public void form_network()
		{
			layers[0]=number_of_input_nodes;
			layers[number_of_layers-1]=number_of_output_nodes;
			for(int i=1;i<number_of_layers-1;i++)
				layers[i]=maximum_layers;
		}
		public void initialize_weights()
		{
			for(int i=1;i<number_of_layers;i++)
				for(int j=0;j<layers[i];j++)
					for(int k=0;k<layers[i-1];k++)
						weight[i,j,k]=(float)(rnd.Next(-weight_bias,weight_bias));
		}
		public void form_input_set()
		{			
			for(int k=0;k<number_of_input_sets;k++)
			{
				get_next_character();
				label16.Text =(k+1).ToString ();
				label16.Update ();
				for(int i=0;i<10;i++)
					for(int j=0;j<15;j++)
					{
						input_set[i*15+j,k]=ann_input_value[i*2+1,j*2+1];						
					}
			}
		}
		public void form_desired_output_set()
		{
			for(int i=0;i<number_of_input_sets;i++)
			{
				character_to_unicode(trainer_string[i].ToString());
				for(int j=0;j<number_of_output_nodes;j++)
					desired_output_set[j,i]=desired_output_bit[j];
			}			
		}		
		public void train_network()
		{									
			int set_number;	
			float average_error=0.0F;
            for (int epoch = 0; epoch <= epochs; epoch++)
            {
                average_error = 0.0F;
                for (int i = 0; i < number_of_input_sets; i++)
                {
                    set_number = rnd.Next(0, number_of_input_sets);
                    get_inputs(set_number);
                    get_desired_outputs(set_number);
                    calculate_outputs();
                    calculate_errors();
                    calculate_weights();
                    average_error = average_error + get_average_error();
                }
                 average_error = average_error / number_of_input_sets;
                if (average_error < error_threshold)
                {
                    epoch = epochs + 1;
                    label22.Text = "<";
                    label22.Update();
                }
            }
						
		}
		public void reset_controls()
		{
			label22.Text="N/A"; label22.Update ();
			}				
		public void get_inputs(int set_number)
		{
			for(int i=0;i<number_of_input_nodes;i++)
				current_input[i]=input_set[i,set_number];
		}
		public void get_desired_outputs(int set_number)
		{
			for(int i=0;i<number_of_output_nodes;i++)
				desired_output[i]=desired_output_set[i,set_number];
		}		
		public void calculate_outputs()
		{
			float f_net;
			int number_of_weights;
			for(int i=0;i<number_of_layers;i++)
				for(int j=0;j<layers[i];j++)
				{
					f_net=0.0F;
					if(i==0) number_of_weights=1;
					else number_of_weights=layers[i-1];
					
					for(int k=0;k<number_of_weights;k++)
						if(i==0)
							f_net=current_input[j];
						else
							f_net=f_net+node_output[i-1,k]*weight[i,j,k];
					node_output[i,j]=sigmoid(f_net);
				}			
		}
		public float sigmoid(float f_net)
		{						
			float result=(float)((2/(1+Math.Exp(-1*slope*f_net)))-1);		//Bipolar			
			return result;
		}
		public float sigmoid_derivative(float result)
		{
			float derivative=(float)(0.5F*(1-Math.Pow(result,2)));			//Bipolar			
			return derivative;
		}
		public int threshold(float val)
		{
			if(val<0.5)					
				return 0;
			else
				return 1;
		}
		public void calculate_errors()
		{
			float sum=0.0F;
			for(int i=0;i<number_of_output_nodes;i++)				
				error[number_of_layers-1,i]=(float)((desired_output[i]-node_output[number_of_layers-1,i])*sigmoid_derivative(node_output[number_of_layers-1,i]));
			for(int i=number_of_layers-2;i>=0;i--)
				for(int j=0;j<layers[i];j++)
				{
					sum=0.0F;
					for(int k=0;k<layers[i+1];k++)
						sum=sum+error[i+1,k]*weight[i+1,k,j];					
					error[i,j]=(float)(sigmoid_derivative(node_output[i,j])*sum);
				}
		}
		public float get_average_error()
		{			
			float average_error=0.0F;
			for(int i=0;i<number_of_output_nodes;i++)			
				average_error=average_error+error[number_of_layers-1,i];
			average_error=average_error/number_of_output_nodes;
			return Math.Abs (average_error);
		}
		public void calculate_weights()
		{
			for(int i=1;i<number_of_layers;i++)
				for(int j=0;j<layers[i];j++)
					for(int k=0;k<layers[i-1];k++)
					{
						weight[i,j,k]=(float)(weight[i,j,k]+learning_rate*error[i,j]*node_output[i-1,k]);						
					}
		}
		public void save_network()
		{
			
		}
		public void load_network()
		{
			form_network();
			openFileDialog1.Filter = "Artificial Neural Network Files (*.ann)|*.ann" ;			
			string line;
			char[] weight_char=new char [20];
			string weight_text="";			
			int title_length,weight_length;
			if((openFileDialog1.ShowDialog() == DialogResult.OK))
			{
				if(openFileDialog1.FileName != "")
				{										
					network_load_file_stream = new StreamReader (openFileDialog1.FileName);								
					network_file_name=Path.GetFileNameWithoutExtension(openFileDialog1.FileName );
					for(int i=0;i<9;i++)
						network_load_file_stream.ReadLine ();					
					for(int i=1;i<number_of_layers;i++)
						for(int j=0;j<layers[i];j++)
							for(int k=0;k<layers[i-1];k++)
							{ 
								weight_text="";
								line=network_load_file_stream.ReadLine();																
								title_length=("Weight["+i.ToString ()+" , "+j.ToString ()+" , "+k.ToString ()+"] = ").Length;
								weight_length=line.Length-title_length;
								line.CopyTo (title_length,weight_char,0,weight_length);
								for(int counter=0;counter<weight_length;counter++)
									weight_text=weight_text+weight_char[counter].ToString ();
								weight[i,j,k] = (float)Convert.ChangeType(weight_text, typeof(float));																
							}					
					network_load_file_stream.Close();
				}				
			}
		}
		public void save_output()
		{
			saveFileDialog1.Filter = "Text Files (*.txt)|*.txt" ;
			saveFileDialog1.DefaultExt="txt";
			saveFileDialog1.FileName =image_file_name;
			if((saveFileDialog1.ShowDialog() == DialogResult.OK))
			{
				if(saveFileDialog1.FileName != "")														
					richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);													
			}
		}		
		public int binary_to_decimal()
		{
			int dec=0;
			for(int i=0;i<number_of_output_nodes;i++)
				dec=dec+output_bit[i]*(int)(Math.Pow (2,i));
			return dec;
		}		
		public void character_to_unicode(string character)
		{			
			int byteCount = unicode.GetByteCount(character.ToCharArray());
			byte[] bytes = new Byte[byteCount];			
			bytes= unicode.GetBytes(character);
			BitArray bits = new BitArray( bytes );
			System.Collections.IEnumerator bit_enumerator = bits.GetEnumerator();
			int bit_array_length = bits.Length;				
			bit_enumerator.Reset ();
			for(int i=0;i<bit_array_length;i++)
			{
				bit_enumerator.MoveNext();						
				if(bit_enumerator.Current.ToString()=="True")
					desired_output_bit[i]=1;
				else
					desired_output_bit[i]=0;				
			}							
		}
		public char unicode_to_character()
		{
			int dec=binary_to_decimal();			
			Byte[] bytes = new Byte[2];			
			bytes[0]=(byte)(dec);
			bytes[1]=0;
			int charCount = unicode.GetCharCount(bytes);
			char[] chars = new Char[charCount];
			chars=unicode.GetChars(bytes);						
			return chars[0];
		}
		public string binary_to_hex()
		{
			int dec;
			string hex="";
			for(int i=3;i>=0;i--)
			{
				dec=0;
				for(int j=3;j>=0;j--)
					dec=dec+(int)(output_bit[i*4+j]*Math.Pow(2,j));
				if(dec>9)
					switch(dec)
					{
						case 10: hex=hex+"A";break;
						case 11: hex=hex+"B";break;
						case 12: hex=hex+"C";break;
						case 13: hex=hex+"D";break;
						case 14: hex=hex+"E";break;
						case 15: hex=hex+"F";break;
					}
				else
					hex=hex+dec.ToString ();
			}
			return hex;
		}
	}
}