using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Security;
using TxTBot.Concretas;

namespace TxTBot
{
    public partial class frmDiretorio : Form
    {
        Compilador compilador = new Compilador();

        public frmDiretorio()
        {
            InitializeComponent();
        }

        private void HabilitarControle(bool habilitar)
        {
            txtDiretorio.Enabled = habilitar;
            btnDiretorio.Enabled = habilitar;
            btnCompilar.Enabled = habilitar;
            btnOneNote.Enabled = habilitar;
        }

        private void btnOneNote_Click(object sender, EventArgs e)
        {
        }

        private void btnDiretorio_Click(object sender, EventArgs e)
        {
            if (fbdDiretorio.ShowDialog() == DialogResult.OK)
            {
                txtDiretorio.Text = fbdDiretorio.SelectedPath;
            }
        }

        private void btnCompilar_Click(object sender, EventArgs e)
        {
            try
            {
                if (fbdDiretorio.SelectedPath != string.Empty)
                {
                    //Bloqueia os controles no inicio do precesso
                    HabilitarControle(false);

                    //Colocar o @ antes do path do arquivo faz com que o compilador não interprete a barra "\" como sendo 
                    //uma mudança de linha ou tabulação (\n ou \t) e sim como uma string.
                    compilador.DiretorioSelecionado = @txtDiretorio.Text;
                    compilador.CompilarTagsParametrizadas(pbArquivos);

                    if (compilador.ProcessoCompleto)
                    {
                        MessageBox.Show("Processo Completado!");

                        //Habilita novamente os controles no final do precesso
                        HabilitarControle(true);
                    }
                }
                else
                {
                    //Não foi selecionado o diretório
                    MessageBox.Show("Favor selecionar o diretório onde se localizam os arquivos a serem compilados!", "Selecione o diretório!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (SecurityException ex)
            {
                //O usuário não possui permissão para ler arquivos
                MessageBox.Show("Erro de segurança, contate o administrador de segurança da rede.\n\n"
                    + "Mensagem : " + ex.Message + "\n\n"
                    + "Detalhes (enviar ao suporte):\n\n" + ex.StackTrace, "Erro de permissão!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro reportado : " + ex.Message + "\nArquivo Atual : " + compilador.ArquivoAtual + "\nLinha Atual : " + compilador.LinhaAtual, "Erro genérico!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
