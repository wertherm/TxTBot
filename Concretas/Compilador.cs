using System;
using System.Linq;
using System.Text;

using System.IO;
using System.Windows.Forms;

using System.Configuration; //Necessário adicionar a referência na solution também.

namespace TxTBot.Concretas
{
    public class Compilador
    {
        public DirectoryInfo DiretorioCompleto { get; set; }
        public Boolean ProcessoCompleto { get; set; }
        public FileInfo[] Arquivos { get; set; }
        public String DiretorioSelecionado { get; set; }
        public String[] Diretorios { get; set; }
        public String ArquivoAtual { get; set; }
        public String LinhaAtual { get; set; }

        private StreamReader Arquivo { get; set; }
        private StreamReader ArquivoDestino { get; set; }
        private StreamWriter ArquivoConteudo { get; set; }

        private String NovoArquivoTxt { get; set; }
        private String DelimitadorEsquerdo { get; set; }
        private String DelimitadorDireito { get; set; }
        private String CoringaNome { get; set; }
        private String RaizDiretorio { get; set; }
        private String RaizLinks { get; set; }
        private String DiretorioParametrizado { get; set; }

        private int Posicao { get; set; }
        private string Linha { get; set; }
        private string Conteudo { get; set; }
        private string ConteudoAnterior { get; set; }
        private string AuxConteudo { get; set; }
        private string AuxDiretorio { get; set; }
        private string Caminho { get; set; }
        private string Extencao { get; set; }

        public Compilador()
        {
        }

        /// <summary>
        ///     Funcionalidade
        ///         Vasculha o diretório selecionado atrás de TxTs com as Tags Parametrizadas.
        ///         Cria as pastas automaticamente que estão nas tags do arquivo no intervalo |%Diretorios!Arquivo%|.
        /// 	    Lê o arquivo de Origem e na tag parametrizada depois do coringa "!" deverá ser colocado o nome do arquivo.
        /// 	
        ///     Links
        ///         Já cria automaticamente o arquivo .url
        ///         Na linha após a tag parametrizada no arquivo de origem deve ficar o título do link e na terceira a Url.
        ///             Ex:
        ///                 |%Pesquisa%|
        ///                 www.google.com
        ///                 http://www.google.com
        ///                 
        ///                 Ou
        ///                 
        ///                 |%Foruns%|
        ///                 forum.macnews.com.br - 206 atalhos para o mac OsX Leopard
        ///                 http://forum.macnews.com.br/topic/45877-206-atalhos-para-o-mac-osx-leopard
        ///                 
        ///         Quando existirem diversos links um em baixo do outro em sequencia, deverá ser criado um Txt dentro de um diretorio para organização posterior:
        /// 	        Ex: |%Tarefas/Organizações/Links!Visualizar%|
        ///     
        ///     Regras
        ///         1. Se no arquivo de Origem tiver "--" como por exemplo em "--Arquivo.txt" na frente de seu nome ou dentro do próprio arquivo como em --|%Tarefas/Organizações/Links!Visualizar%|, este não entrará no processo de organização.
        ///         5. Em links não pode haver tabulação, nem coringas como "/" no título.
        ///         6. Nas Tags prestar atenção se o final não possui tabulação após %| 
        ///         7. Tomar cuidado com erros nas tags como: |%Notas/Pesquisa!Financas!Acoes%|
        ///         8. Preste atenção se o link não possui a estrutura Tag > Titulo > URL, será retornado um erro.
        ///         9. Tome cuidado também para não esquecer o coringa '!' na Tag de um texto, senão será considerado um link e pode ocasionar erro.
        ///         10. Se não exitir ums Tag no arquivo e o restantes das informações não possuirem Tag, o conteúdo do arquivo será atribuido a primeira Tag.
        ///         11. Se for feita duas compilações distintas e os diretórios estiverem com o mesmo nome, a segunda compilação atualizará a primeira. Sem perder informação.
        ///             
        ///    Excessões
        ///         1. Pode haver espaços na Tags, mas não é recomendado é melhor usar underline.
        ///         2. Pode haver mais de um enter entre as Tags, mas não dentro de um bloco, pois estes são organizados por tabulação
        ///         3. Pode haver acentos nas Tags e nos blocos
        /// 
        ///     Referencias
        ///         Método IndexOf
        ///             http://dotnetperls.com/indexof
        ///         Directory.CreateDirectory Method
        ///             http://msdn.microsoft.com/en-us/library/system.io.directory.createdirectory(v=vs.71).aspx
        ///         Método Split
        ///             http://dotnetperls.com/string-split
        ///         Ler um arquivo de texto usando o System.IO e o Visual C# .NET
        ///             http://support.microsoft.com/kb/306777/pt-br
        ///         Leitura e escrita em arquivos com C#
        ///             http://imasters.com.br/artigo/12197/csharp/leitura_e_escrita_em_arquivos_com_c
        ///         Simple Text File Operations in C#
        ///             http://www.csharphelp.com/2005/12/simple-text-file-operations-in-c
        ///         Quebra de Linha em C#
        ///             http://social.msdn.microsoft.com/Forums/pt-BR/clientept/thread/bf79be70-d50a-4438-91ed-591640c01244
        ///             http://social.msdn.microsoft.com/Forums/pt-BR/504/thread/ab8b03ef-e969-4926-9b8d-4eb628e2d9cd
        /// 
        ///     Motivação
        ///         A tecnologia não atende nossas necessidades de ser agil e organizado ao mesmo tempo. Informação deve ser fluída, constante como o 
        ///         pensamento humano e a tecnologia ainda não é muito dinâmica e inteligente o suficiente para acompanhar as mudanças nesta velociadade 
        ///         necessária que dê um retorno rápido e organizado. A informação fluida siginifica toda informação pode estar ligada.
        ///         Lembre-se tablets são só um meio mais prático e bonito de criar informação como antigamente. A tecnologia ainda limita a agidade do pensamento humano.
        ///         
        ///     Tarefas
        ///         Criar a funcionalidade reversa para entrar em diretórios organizados e sincronizar com Favoritos, OneNote / SkyDrive, Google Drive e SQL
        ///         Você pode ainda ter os dados sincronizados com algum blog, suas codificações
        ///         Você pode criar funcionalidades de calendário, para não disponibilizar mais informações para o Google
        ///         Você pode ainda estender a idéia para organizar seus arquivos, vídeos.
        ///         Você poderá gerenciar seus programas também e poderá acessa-los do trabalho em sua casa.
        ///         Você pode sincronizar com o banco de dados informações diretamente de sites acessados
        ///         Você pode criptografar as informações, assim muitos irão usar sua ferramenta. A informação fica armazenada no dispositivo móvel 
        ///         até que seja sincronizada com um PC ou Tablet. Nunca mais a informação ficará na rede. As informações que a pessoa desejasse colocar na 
        ///         internet será só escolher para sincronizar com google drive, skydrive e etc...
        ///         Você pode criar formatação através de códigos para colocar tags HTML, exemplo: |105|Teste|106| = <b>Teste</b>.
        ///         Você pode sincronizar os favoritos, para IE, Chrome e etc...
        ///         
        ///     Observações
        ///         Se você quer suas anotações e links perfeitamente organizados o ideal é armazenar em banco dados só assim você poderá 
        ///         relacionar tópicos relacionados, verbetes e urls agrupados ao mesmo tempo na mesma categoria, sem que se repeta 
        ///         para as outras anotacões.
        ///         
        ///         URls, Imagens e até mesmo vídeos ficariam em tabelas separadas, mas uma anotação poderia ter 
        ///         o link para o vídeo, a imagem. É o mesmo conceito da planilha onde você encontra tudo em um único arquivo. Você fica 
        ///         preso as limitações do software que o projetista criou. Até seu trabalho pode ficar organizado assim e será útil 
        ///         para usar em outras empresas também.
        ///         
        ///         Você pode criar um site para acessar e cadastrar as informações. Suas senhas 
        ///         podem ficar armazenadas aqui também. Você não dependerá de nenhum programa que falhe ou de politicas de seguranças das empresas
        ///         que bloqueiam sites de armazenamento e pen-drives (Transformando você em um robô).
        ///         
        ///         O banco de dados é muito bom, pois não divulgarei minhas informações para o Google. Este sisteminha é a idéia da PessoalNet.
        ///         Pois nem toda idéia deve ser colocada na internet.
        ///         
        ///         Você não precisa mais instalar OneNote, Usar SkyDrive, você criará suas próprias funcionalidades.
        ///         
        ///         Use Linux, Aprenda Java, Desenvolva para Android e sempre se mantenha organizado.
        ///         
        ///     Rotinas
        ///         Você deverá estar constantemente organizando e fazendo backup do banco de dados, enquanto ele não estiver disponível na internet.
        ///         
        ///     Instanciação obsoleta (VS 2010 Express)
        ///         Obsoleta: NameValueCollection _appSettings = ConfigurationManager.AppSettings;
        ///         Nova(Não funciona!): ConfigurationSettings.AppSettings["ExtencaoTXT"]
        ///
        ///     1 - Tem que adicionar a referencia da dll System.Configuration
        ///     2 - adicionar using System.Configuration;
        ///     3 - Tem que trocar ConfigurationSettings.AppSettings por ConfigurationManager.AppSettings
        ///     Resolve System.Configuration.ConfigurationSettings warning
        ///         http://weblogs.asp.net/hosamkamel/archive/2007/09/07/resolve-system-configuration-configurationsettings-warning.aspx
        ///     Configuration Settings File for providing application configuration data
        ///         http://www.codeproject.com/Articles/6538/Configuration-Settings-File-for-providing-applicat
        ///     ConfigurationSettings.AppSettings Property
        ///         http://msdn.microsoft.com/en-us/library/system.configuration.configurationsettings.appsettings.aspx
        ///     .NET Framework V2.0 Obsolete API List
        ///         http://msdn.microsoft.com/en-us/vstudio/Aa497286.aspx
        /// </summary>
        /// <returns></returns>
        public Boolean CompilarTagsParametrizadas(ProgressBar pbArquivos)
        {
            AuxConteudo = string.Empty;

            DiretorioCompleto = new DirectoryInfo(DiretorioSelecionado);
            Arquivos = DiretorioCompleto.GetFiles(ConfigurationManager.AppSettings["ExtencaoTXT"]);

            //Atribui a quantidade de arquivos TXT para o Progress Bar
            pbArquivos.Maximum = Arquivos.Length;

            foreach (FileInfo file in Arquivos)
            {
                //Armazena o nome do arquivo caso ocorra algum erro.
                ArquivoAtual = file.Name;

                //Se o arquivo não conter "--" em seu nome então o processo de organização deve seguir
                if (!file.Name.Contains(ConfigurationManager.AppSettings["CaracterExclusao"]))
                {
                    //Encoding.GetEncoding(1252) implementa acentos
                    Arquivo = new StreamReader(file.FullName, Encoding.GetEncoding(1252));

                    Linha = Arquivo.ReadLine();

                    //Atribui um arquivo lido para a Progress Bar
                    pbArquivos.Step = 1;
                    pbArquivos.PerformStep();

                    while (!Arquivo.EndOfStream)
                    {
                        //Armazena a linha atual do arquivo caso ocorra algum erro.
                        LinhaAtual = Linha;

                        //Verifica se a linha possui o delimitador parametrizado, senão esta linha não será processada
                        if (Linha.Contains(ConfigurationManager.AppSettings["DelimitadorAberto"]) && Linha.Contains(ConfigurationManager.AppSettings["DelimitadorFechado"]))
                        {
                            //Limpa delimitadores do caminho
                            Caminho = Linha.Replace(ConfigurationManager.AppSettings["DelimitadorAberto"], string.Empty).Replace(ConfigurationManager.AppSettings["DelimitadorFechado"], string.Empty);

                            VerificarTextoLink();
                            SepararNomesDiretorios();
                            ManterTextoPrimeiroBloco();
                            CriarDiretoriosEArquivos();
                        }
                        else
                        {
                            throw new Exception(ConfigurationManager.AppSettings["MsgErroDelimitadores"]);
                        }
                    }

                    ProcessoCompleto = true;
                }
            }

            return ProcessoCompleto;
        }

        /// <summary>
        /// Se posição é maior que -1 é porque tem "!" e portando tem arquivo txt para criar e não é um link. Senão é um link.
        /// </summary>
        private void VerificarTextoLink()
        {
            //Recupera o nome do arquivo TXT na Tag e retira ele da variavel caminho
            Posicao = Caminho.IndexOf("!");

            if (Posicao > -1)
            {
                NovoArquivoTxt = Caminho.Substring(Posicao + 1);
                Caminho = Caminho.Substring(0, Posicao);
                NomearExtencaoDiretorio(ConfigurationManager.AppSettings["ExtencaoTXT"], ConfigurationManager.AppSettings["RaizDiretorioParametrizado"]);
            }
            else
            {
                NovoArquivoTxt = Arquivo.ReadLine();
                NomearExtencaoDiretorio(ConfigurationManager.AppSettings["ExtencaoURL"], ConfigurationManager.AppSettings["RaizDiretorioLinksParametrizado"]);
            }
        }

        private void NomearExtencaoDiretorio(string extencao, string diretorio)
        {
            Extencao = extencao;
            DiretorioParametrizado = diretorio;
        }

        /// <summary>
        /// Separa nomes dos diretórios
        /// </summary>
        private void SepararNomesDiretorios()
        {
            Diretorios = Caminho.Split('/');

            foreach (var var in Diretorios)
            {
                Caminho = DiretorioCompleto + DiretorioParametrizado + AuxDiretorio + var;

                //Serve para auxiliar a colocar um diretorio dentro do outro
                AuxDiretorio += var + "\\";
            }
        }

        /// <summary>
        /// Em quanto as próximas linhas não tiverem o delimitador significa que o texto faz parte do primeiro bloco.
        /// </summary>
        private void ManterTextoPrimeiroBloco()
        {
            while (!AuxConteudo.Contains(ConfigurationManager.AppSettings["DelimitadorAberto"]) && !Arquivo.EndOfStream)
            {
                AuxConteudo = Arquivo.ReadLine();

                if (!AuxConteudo.Contains(ConfigurationManager.AppSettings["DelimitadorAberto"]) && AuxConteudo != string.Empty)
                {
                    if (Extencao == ConfigurationManager.AppSettings["ExtencaoTXT"])
                    {
                        Conteudo += AuxConteudo + Environment.NewLine;
                    }
                    else if (Extencao == ConfigurationManager.AppSettings["ExtencaoURL"])
                    {
                        Conteudo += AuxConteudo;
                    }
                }
            }
        }

        /// <summary>
        /// Criação dos diretórios e arquivos txt ou url
        /// </summary>
        private void CriarDiretoriosEArquivos()
        {
            Linha = AuxConteudo;
            Directory.CreateDirectory(Caminho);

            NovoArquivoTxt = TrocarCoringas(NovoArquivoTxt, '-');
            Caminho += "\\" + NovoArquivoTxt + Extencao.Replace("*", string.Empty);

            if (Extencao == ConfigurationManager.AppSettings["ExtencaoTXT"])
            {
                if (!File.Exists(Caminho))
                {
                    TratamentoDeCriacaoDeArquivos(Caminho, Conteudo);
                }
                else
                {
                    ArquivoDestino = new StreamReader(Caminho);

                    while (!ArquivoDestino.EndOfStream)
                    {
                        AuxConteudo = ArquivoDestino.ReadLine();
                        ConteudoAnterior += AuxConteudo + Environment.NewLine;
                    }
                    ArquivoDestino.Close();

                    TratamentoDeCriacaoDeArquivos(Caminho, ConteudoAnterior + Conteudo);
                }
            }
            else if (Extencao == ConfigurationManager.AppSettings["ExtencaoURL"])
            {
                if (!File.Exists(Caminho))
                {
                    EstruturarLink();
                }
            }

            ZerarPropriedades();
        }

        private void EstruturarLink()
        {
            string conteudoLink = string.Empty;

            conteudoLink += ConfigurationManager.AppSettings["EstruturaLink1"];
            conteudoLink += string.Format(ConfigurationManager.AppSettings["EstruturaLink2"], Conteudo);
            conteudoLink += ConfigurationManager.AppSettings["EstruturaLink3"];
            conteudoLink += string.Format(ConfigurationManager.AppSettings["EstruturaLink4"], Conteudo);
            conteudoLink += ConfigurationManager.AppSettings["EstruturaLink5"];
            conteudoLink += ConfigurationManager.AppSettings["EstruturaLink6"];
            conteudoLink += ConfigurationManager.AppSettings["EstruturaLink7"];

            TratamentoDeCriacaoDeArquivos(Caminho, conteudoLink);
        }

        private void TratamentoDeCriacaoDeArquivos(string caminho, string conteudo)
        {
            try
            {
                ArquivoConteudo = File.CreateText(caminho);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            ArquivoConteudo.WriteLine(conteudo);
            ArquivoConteudo.Close();
        }

        /// <summary>
        /// Troca (: ? ! # $ @ * & por -), para corrigir os erros de títulos dos Links
        /// </summary>
        private string TrocarCoringas(string texto, char caracter)
        {
            string retorno = string.Empty;

            //É redundante atrubuir para retorno, sendo que a operação Replace() já altera o valor
            //Mas não foi identificado o porque que em alguns casos o valor não é alterado, por isso
            //foi feito desta forma.
            retorno = texto.Replace(':', caracter);
            retorno = retorno.Replace('?', caracter);
            retorno = retorno.Replace('!', caracter);
            retorno = retorno.Replace('#', caracter);
            retorno = retorno.Replace('$', caracter);
            retorno = retorno.Replace('@', caracter);
            retorno = retorno.Replace('*', caracter);
            retorno = retorno.Replace('&', caracter);

            return retorno;
        }

        private void ZerarPropriedades()
        {
            Caminho = string.Empty;
            Conteudo = string.Empty;
            AuxConteudo = string.Empty;
            AuxDiretorio = string.Empty;
            ConteudoAnterior = string.Empty;
        }

        private void ConectarComOBanco()
        {
        }
    }
}
