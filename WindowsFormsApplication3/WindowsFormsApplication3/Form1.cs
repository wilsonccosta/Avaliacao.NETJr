using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace WindowsFormsApplication3
{

    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            /* VARIAVEIS*/
            Boolean CHECK_PREMIUN;
            DateTime DATA_INICIAL, DATA_FINAL;
            string arquivoEntrada,arquivoSaida, DadosSouth,DadosNorth,DadosWest,buscador, diaSemaPt, 
                    mesPt, inputTXT, outputTXT, datasBuscadas, LOCADORA, CARRO_DISPONIVEL, TIPO_CARRO;

            /*Declaração com inicialização*/
            Boolean ERRO_DATA = false;
            Boolean ERRO_DATA_ANTES = false;
            int QUANTIDADE_DE_PASSAGEIROS = 1;
            int diasSemana = 0;
            int diasFimdeSemana = 0;
            int diasTotal = 0;
            float valorSouth = 0;
            float valorWest = 0;
            float valorNorth = 0;
            float valorFinal = 0;
            string[] DATAS;
            DATAS = new string[100];


            /* Inicialização de variaveis*/
            diaSemaPt = "---";
            mesPt = "---";
            LOCADORA = "---";
            datasBuscadas = "";
            buscador = "Base de Dados não definida!";
            CARRO_DISPONIVEL = "";


            /* Obtendo diretório da Aplicação para localizar a pasta de dados */
            string diretorioAtual = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory.ToString());
            diretorioAtual = diretorioAtual.Remove(diretorioAtual.Length - 9);
            diretorioAtual = diretorioAtual + "Dados";


            /* Setando arquivos de Entrada, Saida e dados */
            arquivoEntrada = diretorioAtual+@"\Entrada.txt";
            arquivoSaida = diretorioAtual + @"\Saida.txt";
            DadosSouth = diretorioAtual + @"\SouthCar.txt";
            DadosNorth = diretorioAtual + @"\NorthCar.txt";
            DadosWest = diretorioAtual + @"\WestCar.txt";


            /*Função de formatação do dia da semana obtido pelo DatePicker para o formato do input*/
            void dataFormatador()
            {
                if (DATA_INICIAL.DayOfWeek == DayOfWeek.Sunday)
                {
                    diaSemaPt = "dom";
                }
                if (DATA_INICIAL.DayOfWeek == DayOfWeek.Monday)
                {
                    diaSemaPt = "seg";
                }
                if (DATA_INICIAL.DayOfWeek == DayOfWeek.Tuesday)
                {
                    diaSemaPt = "ter";
                }
                if (DATA_INICIAL.DayOfWeek == DayOfWeek.Wednesday)
                {
                    diaSemaPt = "qua";
                }
                if (DATA_INICIAL.DayOfWeek == DayOfWeek.Thursday)
                {
                    diaSemaPt = "qui";
                }
                if (DATA_INICIAL.DayOfWeek == DayOfWeek.Friday)
                {
                    diaSemaPt = "sex";
                }
                if (DATA_INICIAL.DayOfWeek == DayOfWeek.Saturday)
                {
                    diaSemaPt = "sab";
                }
            }

            /*Função de formatação do mês obtido pelo DatePicker para o formato do input*/
            void mesFormatador()
            {
                if (DATA_INICIAL.Month == 1)
                {
                    mesPt = "Jan";
                }
                if (DATA_INICIAL.Month == 2)
                {
                    mesPt = "Fev";
                }
                if (DATA_INICIAL.Month == 3)
                {
                    mesPt = "Mar";
                }
                if (DATA_INICIAL.Month == 4)
                {
                    mesPt = "Abr";
                }
                if (DATA_INICIAL.Month == 5)
                {
                    mesPt = "Mai";
                }
                if (DATA_INICIAL.Month == 6)
                {
                    mesPt = "Jun";
                }
                if (DATA_INICIAL.Month == 7)
                {
                    mesPt = "Jul";
                }
                if (DATA_INICIAL.Month == 8)
                {
                    mesPt = "Ago";
                }
                if (DATA_INICIAL.Month == 9)
                {
                    mesPt = "Set";
                }
                if (DATA_INICIAL.Month == 10)
                {
                    mesPt = "Out";
                }
                if (DATA_INICIAL.Month == 11)
                {
                    mesPt = "Nov";
                }
                if (DATA_INICIAL.Month == 12)
                {
                    mesPt = "Dez";
                }
            }

            /* Recebendo dados da Interface */
            DATA_INICIAL = dateTimePicker1.Value;
            DATA_FINAL = dateTimePicker2.Value;
            if (comboBox1.Text != "")
            {
                QUANTIDADE_DE_PASSAGEIROS = Convert.ToInt16(comboBox1.Text);
            }
            CHECK_PREMIUN = checkBox1.Checked;

            /* Verifica se a data de retirada é valida(não é menor que o dia atual)*/
            if ((DATA_INICIAL.Subtract(DateTime.Today).Days)<0)
            {
                ERRO_DATA_ANTES = true;
            }
            
            /*Checka se o usuário é premiun ou não e insere o resultado em
             TIPO_CARRO como Premiun ou Normal*/
            if (CHECK_PREMIUN == true)
            {
                TIPO_CARRO = "Premiun";
            }
            else
            {
                TIPO_CARRO = "Normal";
            }


            /* Convertendo dias para formato a ser inserido no txt
             Também faz a contagem de dias da semana e finais de semana*/
            inputTXT = TIPO_CARRO + ":" + QUANTIDADE_DE_PASSAGEIROS + ":";

            diasTotal = DATA_FINAL.Subtract(DATA_INICIAL).Days;


            /*Verificando se a data de retirada está antes da data de devolução do veiuculo */
            if (diasTotal < 0)
            {
                ERRO_DATA = true;
            }

            for (int i = 0; i <= diasTotal+1 ; i++)
            {

                /* Transformando dia da semana no formato para consulta*/

                dataFormatador();

                /* Transformando mês no formato para consulta*/

                mesFormatador();
                


                DATAS[i] = Convert.ToString(DATA_INICIAL.Day)+ mesPt + Convert.ToString(DATA_INICIAL.Year)+"("+ diaSemaPt + ")";
                

                inputTXT = inputTXT + DATAS[i]+ ","; 
                datasBuscadas = datasBuscadas + DATAS[i] + ",";


                //Conta dias da semana alocados e dias de fim de semana alocados.
                if (DATA_INICIAL.DayOfWeek != DayOfWeek.Sunday &&
                    DATA_INICIAL.DayOfWeek != DayOfWeek.Saturday)
                {
                    diasSemana++;
                }
                else
                {
                    diasFimdeSemana++;
                }

                DATA_INICIAL = DATA_INICIAL.AddDays(1);
            }


            /*                             Calculo de custo do aluguel                              */
            /*                                  tabela de valores                                   */
            /*      SouthCar    Normal:210/diaSemana;200/diaFDS  Premiun:150/diaSemana;90/diaFDS    */
            /*      WestCar     Normal:530/diaSemana;200/diaFDS  Premiun:150/diaSemana;90/diaFDS    */
            /*      NorthCar    Normal:630/diaSemana;600/diaFDS  Premiun:580/diaSemana;590/diaFDS   */
            
            if (TIPO_CARRO == "Normal")
            {
                valorSouth = (210* diasSemana) + (200* diasFimdeSemana);
                valorWest = (530* diasSemana) + (200* diasFimdeSemana);
                valorNorth = (630* diasSemana) + (600* diasFimdeSemana);
            }
            else
            {
                valorSouth = (150 * diasSemana) + (90 * diasFimdeSemana);
                valorWest = (150 * diasSemana) + (90 * diasFimdeSemana);
                valorNorth = (580 * diasSemana) + (590 * diasFimdeSemana);
            }


            if (QUANTIDADE_DE_PASSAGEIROS > 2)
            {
                if (QUANTIDADE_DE_PASSAGEIROS > 4)
                {
                    /* O usuário possui mais de 4 passageiros, logo precisa de um SUV */
                    //buscador será em North
                    valorFinal = valorNorth;
                    LOCADORA = "NorthCar";
                    buscador = DadosNorth;
                }
                else
                {
                    /*Possui entre de 3 a 4 passageiros, pode alugar um carro compacto ou SUV*/
                    if (valorSouth <= valorNorth)
                    {
                        //buscador será em South
                        valorFinal = valorSouth;
                        LOCADORA = "SouthCar";
                        buscador = DadosSouth;
                    }
                    else
                    {
                        //buscador será em North
                        valorFinal = valorNorth;
                        buscador = DadosNorth;
                        LOCADORA = "NorthCar";
                    }
                }
            }
            else
            {
                /* O usuário possui até passageiros, podendo assim alugar qualquer veiculo */
                if (valorSouth <= valorWest && valorSouth <= valorNorth)
                {
                    //buscador será em South
                    valorFinal = valorSouth;
                    buscador = DadosSouth;
                    LOCADORA = "SouthCar";
                }
                if (valorWest <= valorSouth && valorWest <= valorNorth)
                {
                    //buscador será em West
                    valorFinal = valorWest;
                    buscador = DadosWest;
                    LOCADORA = "WestCar";
                }
                if (valorNorth <= valorSouth && valorNorth <= valorWest)
                {
                    //buscador será em North
                    valorFinal = valorNorth;
                    buscador = DadosNorth;
                    LOCADORA = "NorthCar";
                }
            }

            /*Preparando strings de input e output, assim como as datas buscadas */
            if ((ERRO_DATA == false) && (ERRO_DATA_ANTES == false)) { 
                inputTXT = inputTXT.Remove(inputTXT.Length -1);
                datasBuscadas = datasBuscadas.Remove(datasBuscadas.Length - 1);
                outputTXT = CARRO_DISPONIVEL + ":" + LOCADORA;


                /*Sistema de busca na base de dados(txt) com base nos calculos de valor e passageiros obtido*/

                string linhaBusca;
                StreamReader arquivoDados = new StreamReader(buscador);
                while ((linhaBusca = arquivoDados.ReadLine()) != null)
                {
                    //Console.WriteLine(linhaBusca);
                    if (linhaBusca.Contains(datasBuscadas))
                    {
                        var carro = linhaBusca.Split(':');
                        CARRO_DISPONIVEL = carro[0];
                    }
                }
                arquivoDados.Close();


                /* Escrevendo entrada em um arquivo que registra as entradas feitas*/
                StreamWriter escreverEntrada = new StreamWriter(arquivoEntrada, true, Encoding.ASCII);
                escreverEntrada.WriteLine("[" + DateTime.Now + "] " + inputTXT);
                escreverEntrada.Close();

                /* Escrevendo as saidas obtidas em um arquivo*/
                StreamWriter escreverSaida = new StreamWriter(arquivoSaida, true, Encoding.ASCII);
                escreverSaida.WriteLine("[" + DateTime.Now + "] " + outputTXT);
                escreverSaida.Close();

                /* Retornando Dados ao usuário na Interface */
                label8.Text = "Melhor preço encontrado R$: " + valorFinal + ",00";
                label9.Text = "Carro disponível: " + CARRO_DISPONIVEL + " na locadora: " + LOCADORA;


            }
            else
            {
                if (ERRO_DATA) {
                    /* Retornando Erro de data*/
                    label8.Text = "ERRO: DATA DE RETIRADA DEPOIS DE DATA DE DEVOLUÇÃO!";
                    label9.Text = "";
                }
                if (ERRO_DATA_ANTES)
                {
                    /* Retornando Erro de data*/
                    label8.Text = "ERRO: DATA DE RETIRADA ANTERIOR A HOJE!";
                    label9.Text = "";
                }
            }



        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
