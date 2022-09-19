using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agenda
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            mostrar();
            btnDeletar.Visible = false;
            btnUpdate.Visible = false;

        }

        string continua = "yes";

        private void btnInserir_Click(object sender, EventArgs e)
        {
            verificaVazio();

            //Esse comando if com o try e catch vai fazer um se botão inserir for igual a Inserir e continua for yes ele vai fazer um try(um comando para tentar fazer um código) e se não conseguir ele irá fazer um catch(dar uma caixa de texto contendo o erro específicado sem ter que fechar o programa)
            if (btnInserir.Text == "Inserir" && continua == "yes")
            {
                try
                {
                    //Ele vai usar uma conecxão MySql com uma variável cnn que vai receber uma nova conecxão MySql contendo alterações no banco de dados
                    using (MySqlConnection cnn = new MySqlConnection())
                    {
                        cnn.ConnectionString = "server=localhost;database=agenda;uid=root;pwd=;port=3306"; //Aqui ele irá receber um comando Sql que irá fazer a conexão do servidor, database, usuário root a senha e o núrmero da porta 
                        cnn.Open(); //Vai abrir essa variável
                        MessageBox.Show("Inserido com sucesso!");  //Vai mostrar uma caixa de texto com uma mensagem escrita Inserido com sucesso!
                        string sql = "insert into contatos (nome, email) values ('" + txtNome.Text + "', '" + txtEmail.Text + "')";  //Uma variável sql do tipo string que vai recber comando Sql para inserir dentro de contatos nome, email com os valores do txtNome e txtEmail
                        MySqlCommand cmd = new MySqlCommand(sql, cnn); //Ele vai fazer um comando para inserir tudo no banco de dados no mysql então ele vai criar uma variável cmd que vai receber um novo comando Mysql que vai conter as variáveis sql e cnn
                        cmd.ExecuteNonQuery(); //A variável cmd não vai retornar instruções Sql de dados, Insert, Update, Delete, e Set.

                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.ToString());
                }
            }
            txtPesquisar.Clear(); //O txtPesquisar vai receber clear que vai limpar tudo o que foi dígitado no txt
            //Vai trazer o métodos mostrar e limpar que ao traze-los irá executar todo o código inserido neles
            mostrar();
            limpar();
        }

        private void dgwTabela_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgwTabela.CurrentRow.Index != -1)
            {
                txtId.Text = dgwTabela.CurrentRow.Cells[0].Value.ToString();
                txtNome.Text = dgwTabela.CurrentRow.Cells[1].Value.ToString();
                txtEmail.Text = dgwTabela.CurrentRow.Cells[2].Value.ToString();

                btnDeletar.Visible = true;
                btnUpdate.Visible = true;
                btnInserir.Text = "Novo";
                txtPesquisar.Clear();

            }
        }


        void mostrar()
            //Ele irá tentar pegar e mostrar todos os dados do contatos no banco de dados Mysql e vai jogar no painel para mostrar eles
        {
            try
            {
                using (MySqlConnection cnn = new MySqlConnection())
                {
                    cnn.ConnectionString = "server=localhost;database=agenda;uid=root;pwd=;port=3306";
                    cnn.Open();
                    string sql = "Select * from contatos";
                    DataTable table = new DataTable();
                    MySqlDataAdapter adpter = new MySqlDataAdapter(sql, cnn);  //Ele pega e 
                    adpter.Fill(table);
                    dgwTabela.DataSource = table;
                    
                    dgwTabela.AutoGenerateColumns = false;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }


        }

        void limpar()
        {
            //Ele irá pegar os txts Id,Nome e Email e vai limpalos para não aparecer nada neles e btnInserir.Text vai receber um texto Inserir e os btns Deletar e Update vão receber um false que farão eles ficarem invisíveis
            txtId.Text = "";
            txtNome.Clear();
            txtEmail.Text = "";
            btnInserir.Text = "Inserir";
            btnDeletar.Visible = false;
            btnUpdate.Visible = false;



        }

        void verificaVazio()
        //Esse comando vai verificar se o txtNome ou txtEmail for igual a "" (em branco sem dígitos) ele vai fazer com que apareça uma caixa de texto falando para preencher todos os campos se não ele vai continuar normalmente
        {
            if (txtNome.Text == "" || txtEmail.Text == "")
            {
                continua = "no";
                MessageBox.Show("Preencha todos os campos");
            }
            else
            {
                continua = "yes";

            }

        }

        private void btnDeletar_Click(object sender, EventArgs e)
        {
            //Ele irá fazer if DialogResult.Yes(resultado) que se for igual a caixa de texto escrito Deseja realmente excluir, Confirmação e com botões yes e no e se clicar no yes ele vai deletar um id de um contato e se clicar no no ele vai voltar e não irá deletar
            if (DialogResult.Yes == MessageBox.Show("Deseja realmente excluir", "Confirmação", MessageBoxButtons.YesNo))
            {

                try
                {
                    using (MySqlConnection cnn = new MySqlConnection())
                    {
                        cnn.ConnectionString = "server=localhost;database=agenda;uid=root;pwd=;port=3306";
                        cnn.Open();
                        string sql = "Delete from contatos where idContatos = '" + txtId.Text + "'";
                        MySqlCommand cmd = new MySqlCommand(sql, cnn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show(" Deletado com sucesso! ");

                    }
                    limpar();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            txtPesquisar.Clear();
            mostrar();
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            //Ele irá fazer if DialogResult.Yes(resultado) que se for igual a caixa de texto escrito Deseja realmente Alterar, Confirmação e com botões yes e no e se clicar no yes ele vai alterar o nome ou email de um contato e se clicar no no ele vai voltar e não irá alterar
            if (DialogResult.Yes == MessageBox.Show("Deseja realmente alterar", "Confirmação", MessageBoxButtons.YesNo))
            {
                try
                {
                    using (MySqlConnection cnn = new MySqlConnection())
                    {
                        cnn.ConnectionString = "server=localhost;database=agenda;uid=root;pwd=;port=3306";
                        cnn.Open();
                        string sql = "Update contatos set nome='" + txtNome.Text + "', email='" + txtEmail.Text + "' where idContatos='" + txtId.Text + "'";
                        MySqlCommand cmd = new MySqlCommand(sql, cnn);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Atualizado com sucesso!");
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                txtPesquisar.Clear();
                mostrar();
                verificaVazio();

            }
        }
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            //Ele vai criar um botão e duas bolinhas em baixo para clicar e quando clicado ele vai fazer uma conexão com o banco e vai impor uma condição (Like busca por uma expressão como uma letra ele pode ser usado para pesquisa)
            try
            {
                using (MySqlConnection cnn = new MySqlConnection())
                {
                    cnn.ConnectionString = "server=localhost;database=agenda;uid=root;pwd=;port=3306";
                    cnn.Open();
                    string sql;



                    if (rbEmail.Checked)
                    {
                        sql = "Select * from contatos where email Like'" + txtPesquisar.Text + "%'";  //Se clicar aqui ele vai buscar por somente nomes e vai mostrar na tabela quando pesquisado
                    }
                    else
                    {
                        sql = "Select * from contatos where nome Like'" + txtPesquisar.Text + "%'";  //Se ele clicar aqui ele vai buscar por somente emails e vai mostrar na tabela quando pesquisado
                    }






                    MySqlCommand cmd = new MySqlCommand(sql, cnn);
                    cmd.ExecuteNonQuery();
                    DataTable table = new DataTable();
                    MySqlDataAdapter adpter = new MySqlDataAdapter(sql, cnn);
                    adpter.Fill(table);
                    dgwTabela.DataSource = table;

                    dgwTabela.AutoGenerateColumns = false;


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void txtPesquisar_TextChanged(object sender, EventArgs e)
        {
            //Aqui ele também faz todas as funções do btnPesquisar mas trás um pequeno diferencial ele vai checar se 
            try
            {
                using (MySqlConnection cnn = new MySqlConnection())
                {
                    cnn.ConnectionString = "server=localhost;database=agenda;uid=root;pwd=;port=3306";
                    cnn.Open();
                    string sql;

                    if (rbEmail.Checked)
                    {
                        sql = "Select * from contatos where email Like'" + txtPesquisar.Text + "%'";
                    }
                    else
                    {
                        sql = "Select * from contatos where nome Like'" + txtPesquisar.Text + "%'";
                    }

                    MySqlCommand cmd = new MySqlCommand(sql, cnn);
                    cmd.ExecuteNonQuery();
                    DataTable table = new DataTable();
                    MySqlDataAdapter adpter = new MySqlDataAdapter(sql, cnn);
                    adpter.Fill(table);
                    dgwTabela.DataSource = table;
                    dgwTabela.AutoGenerateColumns = false;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}