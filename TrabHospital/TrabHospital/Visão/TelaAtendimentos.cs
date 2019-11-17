﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrabHospital.Controladora;

namespace TrabHospital.Visão
{
	public partial class TelaAtendimentos : Form
	{
        private CtrlAtendimentos ControlAte = new CtrlAtendimentos();
        private DataTable dtConta = new DataTable();
        DataTable dtatends = new DataTable();

        public TelaAtendimentos()
		{
			InitializeComponent();
            dtConta.Columns.Add("pro_descricao");
            dtConta.Columns.Add("con_data");
            dtConta.Columns.Add("con_qtde");
            dtConta.Columns.Add("pro_valor");
            dtConta.Columns.Add("pro_total");
            dtConta.Columns.Add("pro_codigo");
        }

        private void BtnAlterar_Click(object sender, EventArgs e)
        {
            if (dgvAtendimentos.SelectedRows.Count > 0)
            {
                if (dtatends.Rows[dgvAtendimentos.CurrentRow.Index]["atn_contafechada"].ToString() == "N")
                {
                    limpa();
                    tbCodigo.Text = dtatends.Rows[dgvAtendimentos.CurrentRow.Index]["atn_codigo"].ToString();
                    tbAnamnese.Text = dtatends.Rows[dgvAtendimentos.CurrentRow.Index]["atn_anamnese"].ToString();
                    dtpAtendimento.Value = Convert.ToDateTime(dtatends.Rows[dgvAtendimentos.CurrentRow.Index]["atn_data"]);
                    cbDiagnostico.SelectedValue = (int)dtatends.Rows[dgvAtendimentos.CurrentRow.Index]["dia_codigo"];
                    cbPaciente.SelectedValue = (int)dtatends.Rows[dgvAtendimentos.CurrentRow.Index]["pac_codigo"];
                    cbMedico.SelectedValue = (int)dtatends.Rows[dgvAtendimentos.CurrentRow.Index]["med_codigo"];
                    dgvProcedimentos.DataSource = ControlAte.BuscarContas(Convert.ToInt32(tbCodigo.Text));
                    pnDados.Enabled = true;
                    btnNovo.Enabled = false;
                    btnSalvar.Enabled = true;
                    btnCancelar.Enabled = true;
                    cbDiagnostico.Focus();
                    if (dtatends.Rows[dgvAtendimentos.CurrentRow.Index]["atn_dtobito"] != DBNull.Value)
                    {
                        dtpaltaobito.Visible = true;
                        lblaltaobito.Visible = true;
                        tbcausamorte.Visible = true;
                        lblcausamorte.Visible = true;
                    }
                    else if(dtatends.Rows[dgvAtendimentos.CurrentRow.Index]["atn_dtobito"] != DBNull.Value)
                    {
                        dtpaltaobito.Visible = true;
                        lblaltaobito.Visible = true;
                        lblretorno.Visible = true;
                        dtpretorno.Visible = true;
                    }

                    tabsatendimento.SelectedIndex = 0;
                }
            }

        }

        private void BtAdd_Click(object sender, EventArgs e)
        {
            DataRow row = dtConta.NewRow();
            row["pro_descricao"] = cbProcede.Text;
            row["con_data"] = dtpDataConta.Value;
            row["con_qtde"] = tbQtde.Text;
            row["pro_valor"] = tbValor.Text;
            row["pro_codigo"] = cbProcede.SelectedValue;
            row["pro_total"] = Convert.ToDouble(tbValor.Text) * Convert.ToDouble(tbQtde.Text);

            dtConta.Rows.Add(row);
            ControlAte.AddConta(row);
        }

        private void TelaAtendimentos_Load(object sender, EventArgs e)
        {
            cbDiagnostico.DataSource = ControlAte.BuscaDiagnosticos();
            cbDiagnostico.ValueMember = "dia_codigo";
            cbDiagnostico.DisplayMember = "dia_descricao";
            cbPaciente.ValueMember = "pac_codigo";
            cbPaciente.DisplayMember = "pac_nome";
            cbPaciente.DataSource = ControlAte.BuscaPacientes();
            cbMedico.ValueMember = "med_codigo";
            cbMedico.DisplayMember = "med_nome";
            cbMedico.DataSource = ControlAte.BuscaMedicos(Convert.ToInt32(cbPaciente.SelectedValue));
            cbMedico2.ValueMember = "med_codigo";
            cbMedico2.DisplayMember = "med_nome";
            cbMedico2.DataSource = ControlAte.BuscaMedicos2(Convert.ToInt32(cbPaciente.SelectedValue));
            cbProcede.ValueMember = "pro_codigo";
            cbProcede.DisplayMember = "pro_descricao";
            cbProcede.DataSource = ControlAte.BuscarProcedimentos();
            dgvProcedimentos.DataSource = dtConta;
            rbatendimento.Checked = true;
            dgvAtendimentos.DataSource = dtatends = ControlAte.BuscaAtendimentosPData(dtpPeriodo.Value, dtpPeriodoObito.Value, 'a');
        }

        public void limpa()
        {
            tbAnamnese.Clear();
            tbCodigo.Clear();
            tbPesqNomePac.Clear();
            dtConta.Rows.Clear();
            tbQtde.Clear();
            tbValor.Clear();
            dtpaltaobito.Visible = false;
            lblaltaobito.Visible = false;
            tbcausamorte.Visible = false;
            lblcausamorte.Visible = false;
            lblretorno.Visible = false;
            dtpretorno.Visible = false;
        }

        private void BtnNovo_Click(object sender, EventArgs e)
        {
            pnDados.Enabled = true;
            btnNovo.Enabled = false;
            btnSalvar.Enabled = true;
            btnCancelar.Enabled = true;
            cbDiagnostico.Focus();
        }

        private void BtnSalvar_Click(object sender, EventArgs e)
        {
            if(tbAnamnese.Text.Length == 0)
            {
                MessageBox.Show("Campo anamnese deve ser preenchido!","Obrigatório!",
                                MessageBoxButtons.OK,MessageBoxIcon.Warning);
                tbAnamnese.Focus();
            }
            else if(tbCodigo.Text.Length == 0)
            {
                if(ControlAte.SalvarAtendimento((int)cbDiagnostico.SelectedValue,(int)cbPaciente.SelectedValue,
                                (int)cbMedico.SelectedValue,dtpAtendimento.Value,tbAnamnese.Text))
                {
                    limpa();
                    pnDados.Enabled = false;
                    btnNovo.Enabled = true;
                    btnSalvar.Enabled = false;
                    btnCancelar.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Não foi possivel salvar");
                }
            }
            else
            {

            }
        }

        private void BtnCancelar_Click(object sender, EventArgs e)
        {
            limpa();
            pnDados.Enabled = false;
            btnNovo.Enabled = true;
            btnSalvar.Enabled = false;
            btnCancelar.Enabled = false;
        }

        private void BtnVoltar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnPesquisar_Click(object sender, EventArgs e)
        {
            if(rbatendimento.Checked)
            {
                if (tbPesqNomePac.TextLength > 0 && cbMedico2.SelectedIndex == -1)
                {
                    dgvAtendimentos.DataSource = dtatends = ControlAte.BuscaAtendimentosPNomeData(tbPesqNomePac.Text, dtpPeriodo.Value, dtpPeriodoObito.Value, 'a');
                }
                else if (tbPesqNomePac.TextLength > 0 && cbMedico2.SelectedIndex != -1)
                {
                    dgvAtendimentos.DataSource = dtatends = ControlAte.BuscaAtendimentosPNomeDataMed(tbPesqNomePac.Text, dtpPeriodo.Value, dtpPeriodoObito.Value, Convert.ToInt32(cbMedico2.SelectedValue),'a');
                }
                else if (tbPesqNomePac.TextLength == 0 && cbMedico2.SelectedIndex != -1)
                {
                    dgvAtendimentos.DataSource = dtatends = ControlAte.BuscaAtendimentosPDataMed(dtpPeriodo.Value, dtpPeriodoObito.Value, Convert.ToInt32(cbMedico2.SelectedValue),'a');
                }
                else if (tbPesqNomePac.TextLength == 0 && cbMedico2.SelectedIndex == -1)
                {
                    dgvAtendimentos.DataSource = dtatends = ControlAte.BuscaAtendimentosPData(dtpPeriodo.Value, dtpPeriodoObito.Value,'a');
                }
            }
            else if(rbobito.Checked)
            {
                if (tbPesqNomePac.TextLength > 0 && cbMedico2.SelectedIndex == -1)
                {
                    dgvAtendimentos.DataSource = dtatends = ControlAte.BuscaAtendimentosPNomeData(tbPesqNomePac.Text, dtpPeriodo.Value, dtpPeriodoObito.Value,'o');
                }
                else if (tbPesqNomePac.TextLength > 0 && cbMedico2.SelectedIndex != -1)
                {
                    dgvAtendimentos.DataSource = dtatends = ControlAte.BuscaAtendimentosPNomeDataMed(tbPesqNomePac.Text, dtpPeriodo.Value, dtpPeriodoObito.Value, Convert.ToInt32(cbMedico2.SelectedValue),'o');
                }
                else if (tbPesqNomePac.TextLength == 0 && cbMedico2.SelectedIndex != -1)
                {
                    dgvAtendimentos.DataSource = dtatends = ControlAte.BuscaAtendimentosPDataMed(dtpPeriodo.Value, dtpPeriodoObito.Value, Convert.ToInt32(cbMedico2.SelectedValue),'o');
                }
                else if (tbPesqNomePac.TextLength == 0 && cbMedico2.SelectedIndex == -1)
                {
                    dgvAtendimentos.DataSource = dtatends = ControlAte.BuscaAtendimentosPData(dtpPeriodo.Value, dtpPeriodoObito.Value,'o');
                }
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            if(dgvAtendimentos.CurrentRow.Cells[13].Value.ToString() == "S")
            {
                MessageBox.Show("Atendimento já fechado, não é possivel fazer a exclusão", "Atendimento Fechado",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else 
            {
                if (MessageBox.Show("Deseja mesmo excluir o atendimento?", "Alerta!",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    if (!ControlAte.ExcluiAtendimento((int)dtatends.Rows[dgvAtendimentos.CurrentRow.Index]["atn_codigo"]))
                        MessageBox.Show("Erro na exclusão!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                        dtatends.Rows.RemoveAt(dgvAtendimentos.CurrentRow.Index);
            }
        }

        private void BtnLimpar_Click(object sender, EventArgs e)
        {
            rbatendimento.Checked = true;
            tbPesqNomePac.Text = "";
            cbMedico2.SelectedIndex = -1;
            dgvAtendimentos.DataSource = dtatends = ControlAte.BuscaAtendimentosPData(dtpPeriodo.Value, dtpPeriodoObito.Value, 'a');
        }

        private void BtRemover_Click(object sender, EventArgs e)
        {
            if(dgvProcedimentos.SelectedRows.Count>0)
            {
                ControlAte.RemoveConta(dtConta.Rows[dgvAtendimentos.CurrentRow.Index]);
                dtConta.Rows[dgvProcedimentos.CurrentRow.Index].Delete();
            }
        }

        private void cbPaciente_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbMedico.DataSource = ControlAte.BuscaMedicos(Convert.ToInt32(cbPaciente.SelectedValue));
        }

        private void tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbMedico2.SelectedIndex = -1;
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void cbMedico2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbProcede_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbValor.Text = ControlAte.BuscaValorConta((int)cbProcede.SelectedValue).ToString();
        }
    }
}
