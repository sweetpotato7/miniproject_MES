using System;
using DC00_assm;
using DC_POPUP;

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using System.Diagnostics.Contracts;
using System.Net;
using DC00_WinForm;
using System.Web;
using System.Data.Common;

namespace KFQB_Form
{
    public partial class QM_BadProdQM : DC00_WinForm.BaseMDIChildForm
    {

        private UltraGridUtil _GridUtil = new UltraGridUtil();
        DataTable rtnDtTemp = new DataTable();
        Common _Common = new Common();

        public QM_BadProdQM()
        {
            InitializeComponent();
        }

        private void QM_BadProdQM_Load(object sender, EventArgs e)
        {
            #region<그리트 셋팅>
            // 그리드 셋팅을 위한
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "DELFLAG", "폐기", true, GridColDataType_emu.CheckBox, 100, 120, Infragistics.Win.HAlign.Left, true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "REMARK", "사유", true, GridColDataType_emu.VarChar, 140, 120, Infragistics.Win.HAlign.Left, true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "공장", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKERID", "작업자", true, GridColDataType_emu.VarChar, 180, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품목", true, GridColDataType_emu.VarChar, 150, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품명", true, GridColDataType_emu.VarChar, 200, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "FIRSTBADQTY", "초기불량수량", true, GridColDataType_emu.Double, 120, 120, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "BADQTY", "불량수량", true, GridColDataType_emu.Double, 120, 120, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "CHKNO", "판정순번", true, GridColDataType_emu.VarChar, 100, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKER", "생성자", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE", "생성일자", true, GridColDataType_emu.DateTime24, 150, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ORDERNO", "ORDERNO", true, GridColDataType_emu.VarChar, 150, 120, Infragistics.Win.HAlign.Left, false, false);
            _GridUtil.InitColumnUltraGrid(grid1, "INDATESEQ", "INDATESEQ", true, GridColDataType_emu.VarChar, 150, 120, Infragistics.Win.HAlign.Left, false, false);
            _GridUtil.InitColumnUltraGrid(grid1, "LOTNO", "LOTNO", true, GridColDataType_emu.VarChar, 150, 120, Infragistics.Win.HAlign.Left, false, false);
            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            // 사업장
            rtnDtTemp = _Common.Standard_CODE("PLANTCODE");
            Common.FillComboboxMaster(this.cboPlantCode, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            // 작업장코드
            rtnDtTemp = _Common.GET_Workcenter_Code();
            Common.FillComboboxMaster(this.cboWorkcenterCode, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");

            // 사유 콤보박스
            DataTable rtnDtTemp2 = new DataTable();
            rtnDtTemp2.Columns.Add("REMARK");
            rtnDtTemp2.Rows.Add("원자재 불량");
            rtnDtTemp2.Rows.Add("공정 불량");
            UltraGridUtil.SetComboUltraGrid(this.grid1, "REMARK", rtnDtTemp2, "REMARK", "REMARK");
        }

        public override void DoInquire()  //조회수행
        {
            // 조회 버튼 클릭
            DBHelper helper = new DBHelper();
            try
            {
                string sPlantCode = Convert.ToString(cboPlantCode.Value);
                string sWorkCenterCode = Convert.ToString(cboWorkcenterCode.Value);
                string sStartDate = string.Format("{0:yyyy-MM-dd}", dtStart.Value);
                string sEndDate = string.Format("{0:yyyy-MM-dd}", dtEnd.Value);

                DataTable dtTemp = new DataTable();
                dtTemp = helper.FillTable("99QM_BadProdQM_S1", CommandType.StoredProcedure,
                                helper.CreateParameter("PLANTCODE", sPlantCode, DbType.String, ParameterDirection.Input),
                                helper.CreateParameter("WORKCENTERCODE", sWorkCenterCode, DbType.String, ParameterDirection.Input),
                                helper.CreateParameter("STARTDATE", sStartDate, DbType.String, ParameterDirection.Input),
                                helper.CreateParameter("ENDDATE", sEndDate, DbType.String, ParameterDirection.Input)
                                );
                if (dtTemp.Rows.Count == 0)
                {
                    _GridUtil.Grid_Clear(grid1);  // 그리드의 내용을 초기화 한다.
                    ShowDialog("조회할 데이터가 없습니다.", DC00_WinForm.DialogForm.DialogType.OK);
                    return;
                }
                this.grid1.DataSource = dtTemp;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                helper.Close();
            }
        }
        private void btnProdIn_Click(object sender, EventArgs e)
        {
            #region
            #endregion
            DBHelper helper = new DBHelper("", true);
            try
            {
                // 생산 실적 등록 처리
                string sPlnatCode = Convert.ToString(this.grid1.ActiveRow.Cells["PLANTCODE"].Value);
                // 남은 최대 수량 BADQTY (DOUBLE 변환)
                string sBadQTY = Convert.ToString(this.grid1.ActiveRow.Cells["BADQTY"].Value).Replace(",", "");
                double dBadQTY = 0;
                double.TryParse(sBadQTY, out dBadQTY);
                // 입력한 양품 수량
                double dTProdQty = Convert.ToDouble(txtProductQty.Text);


                if (dBadQTY < dTProdQty)
                {
                    ShowDialog("양품 수량이 남은 수량보다 많습니다.", DC00_WinForm.DialogForm.DialogType.OK);
                    return;
                }
                //if(Convert.ToString(grid1.ActiveRow.Cells["DELFLAG"].Value) == "1" && Convert.ToString(grid1.ActiveRow.Cells["REMARK"].Value) == null)
                //{
                //    ShowDialog("사유를 입력해 주세요.", DC00_WinForm.DialogForm.DialogType.OK);
                //}
                helper.ExecuteNoneQuery("99QM_BadProdQM_I1", CommandType.StoredProcedure
                                    , helper.CreateParameter("PLANTCODE", Convert.ToString(grid1.ActiveRow.Cells["PLANTCODE"].Value), DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("INDATESEQ", Convert.ToString(grid1.ActiveRow.Cells["INDATESEQ"].Value), DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("PRODQTY", dTProdQty, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("DELFLAG", Convert.ToString(grid1.ActiveRow.Cells["DELFLAG"].Value), DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("REMARK", Convert.ToString(grid1.ActiveRow.Cells["REMARK"].Value), DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("ORDERNO", Convert.ToString(grid1.ActiveRow.Cells["ORDERNO"].Value), DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("WORKCENTERCODE", Convert.ToString(grid1.ActiveRow.Cells["WORKCENTERCODE"].Value), DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("ITEMCODE", Convert.ToString(grid1.ActiveRow.Cells["ITEMCODE"].Value), DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("LOTNO", Convert.ToString(grid1.ActiveRow.Cells["LOTNO"].Value), DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("WORKERID", LoginInfo.UserID, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("BADQTY", Convert.ToString(grid1.ActiveRow.Cells["BADQTY"].Value), DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("FIRSTBADQTY", Convert.ToString(grid1.ActiveRow.Cells["FIRSTBADQTY"].Value), DbType.String, ParameterDirection.Input)
                                    );
                if (helper.RSCODE == "S")
                {
                    helper.Commit();
                    ShowDialog("생산 실적 등록을 완료하였습니다.");
                    DoInquire();
                }
                else
                {
                    helper.Rollback();
                    ShowDialog(helper.RSMSG);
                }
            }
            catch (Exception ex)
            {
                helper.Rollback();
                ShowDialog(ex.ToString());
            }
            finally
            {
                helper.Close();
            }
        }

        public override void DoSave()
        {
            DataTable dt = new DataTable();

            dt = grid1.chkChange();
            if (dt == null)
                return;
            DBHelper helper = new DBHelper("", false);

            try
            {
                //base.DoSave();

                if (this.ShowDialog("폐기하시겠습니까?") == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["DELFLAG"]) == "0") continue;
                    if (Convert.ToString(dt.Rows[i]["REMARK"]) == "")
                    {
                        ShowDialog("사유를 입력해 주세요.", DC00_WinForm.DialogForm.DialogType.OK);
                        return;
                    }

                    helper.ExecuteNoneQuery("99QM_BadProdQM_D1"
                                            , CommandType.StoredProcedure
                                            , helper.CreateParameter("PLANTCODE", Convert.ToString(dt.Rows[i]["PLANTCODE"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("INDATESEQ", Convert.ToString(dt.Rows[i]["INDATESEQ"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("ORDERNO", Convert.ToString(dt.Rows[i]["ORDERNO"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("WORKCENTERCODE", Convert.ToString(dt.Rows[i]["WORKCENTERCODE"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("ITEMCODE", Convert.ToString(dt.Rows[i]["ITEMCODE"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("LOTNO", Convert.ToString(dt.Rows[i]["LOTNO"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("BADQTY", Convert.ToString(dt.Rows[i]["BADQTY"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("FIRSTBADQTY", Convert.ToString(dt.Rows[i]["FIRSTBADQTY"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("REMARK", Convert.ToString(dt.Rows[i]["REMARK"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("WORKERID", LoginInfo.UserID, DbType.String, ParameterDirection.Input));

                    if (helper.RSCODE == "E")
                    {
                        this.ShowDialog(helper.RSMSG, DialogForm.DialogType.OK);
                        helper.Rollback();
                        return;
                    }
                }

                helper.Commit();
                this.ShowDialog("데이터가 저장 되었습니다.", DialogForm.DialogType.OK);
                DoInquire();
            }
            catch (Exception ex)
            {
                helper.Rollback();
                ShowDialog(ex.ToString());
            }
            finally
            {
                helper.Close();
            }
        }
    }
}

