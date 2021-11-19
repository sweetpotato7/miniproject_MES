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
    public partial class PP_ActureOutPut : DC00_WinForm.BaseMDIChildForm
    {

        private UltraGridUtil _GridUtil = new UltraGridUtil();
        DataTable rtnDtTemp = new DataTable();
        Common _Common = new Common();
        bool sFormLoad = true; //화면이 로드 될떄 여부를 확인

        public PP_ActureOutPut()
        {
            InitializeComponent();
        }

        private void PP_ActureOutPut_Load(object sender, EventArgs e)
        {
            #region<그리트 셋팅>
            // 그리드 셋팅을 위한
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "공장", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, false, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장 코드", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, false, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERNAME", "작업장 명", true, GridColDataType_emu.VarChar, 150, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ORDERNO", "작업지시번호", true, GridColDataType_emu.VarChar, 180, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품목", true, GridColDataType_emu.VarChar, 150, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품명", true, GridColDataType_emu.VarChar, 200, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ORDERQTY", "지시수량", true, GridColDataType_emu.Double, 100, 120, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "PRODQTY", "양품수량", true, GridColDataType_emu.Double, 100, 120, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "BADQTY", "불량수량", true, GridColDataType_emu.Double, 100, 120, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "UNITCODE", "단위", true, GridColDataType_emu.VarChar, 100, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKSTATUSCODE", "가동/비가동코드", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, false, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKSTATUS", "상태(가동/비가동)", true, GridColDataType_emu.VarChar, 100, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MATLOTNO", "투입LOT", true, GridColDataType_emu.VarChar, 200, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "COMPONENTQTY", "투입잔량", true, GridColDataType_emu.Double, 100, 120, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKER", "작업자코드", true, GridColDataType_emu.VarChar, 100, 120, Infragistics.Win.HAlign.Left, false, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKERNAME", "작업자명", true, GridColDataType_emu.VarChar, 100, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "STARTDATE", "지시시작일시", true, GridColDataType_emu.DateTime, 220, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ENDDATE", "지시종료일시", true, GridColDataType_emu.DateTime, 220, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion



            // 사업장
            rtnDtTemp = _Common.Standard_CODE("PLANTCODE");
            Common.FillComboboxMaster(this.cboPlantCode, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            // 작업장코드
            rtnDtTemp = _Common.GET_Workcenter_Code();
            Common.FillComboboxMaster(this.cboWorkcenterCode, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");

            // 단위
            rtnDtTemp = _Common.Standard_CODE("UNITCODE");
            UltraGridUtil.SetComboUltraGrid(this.grid1, "UNITCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            // 작업자 팝업 호출
            BizTextBoxManager txtBiz = new BizTextBoxManager();
            txtBiz.PopUpAdd(txtWorkerID, txtWorkerName, "WORKER_MASTER", new object[] { "", "", "", "", "" });

            sFormLoad = false;
            cboWorkcenterCode_ValueChanged(null, null);

        }

        private void cboWorkcenterCode_ValueChanged(object sender, EventArgs e)
        {
            // 작업장 변경 시 작업장 현재 상태 내역 조회
            if (sFormLoad == true) return;

            DBHelper helper = new DBHelper(false);
            try
            {
                _GridUtil.Grid_Clear(grid1);

                string sPlantCode = Convert.ToString(cboPlantCode.Value);
                string sWorkcneterCode = Convert.ToString(cboWorkcenterCode.Value);

                rtnDtTemp = helper.FillTable("19PP_ActureOutPut_S1", CommandType.StoredProcedure,
                                        helper.CreateParameter("PLANTCODE", sPlantCode, DbType.String, ParameterDirection.Input),
                                        helper.CreateParameter("WORKCENTERCODE", sWorkcneterCode, DbType.String, ParameterDirection.Input)
                                        );
                this.grid1.DataSource = rtnDtTemp;
            }
            catch (Exception ex)
            {
                ShowDialog(ex.ToString(), DialogForm.DialogType.OK);
            }
            finally
            {
                helper.Close();
            }
        }

        private void btnOrderSelect_Click(object sender, EventArgs e)
        {
            // 작업 지시 선택 팝업 호출
            if (grid1.Rows.Count == 0) return;
            if (grid1.ActiveRow == null) return;

            // 작업자 등록 상태 화인
            string sWorkerId = Convert.ToString(grid1.ActiveRow.Cells["WORKER"].Value);
            if (sWorkerId == null)
            {
                ShowDialog("현재 작업장에 등록된 작업자가 없습니다.\r\n작업자 등록 후 진행하세요", DialogForm.DialogType.OK);
                return;
            }
            string sRunStop = Convert.ToString(grid1.ActiveRow.Cells["WORKSTATUSCODE"].Value);
            if (sRunStop == "R")
            {
                ShowDialog("작업장이 현재 가동중입니다.\r\n비가동 등록 후 진행하세요", DialogForm.DialogType.OK);
                return;
            }
            string sLotNo = Convert.ToString(grid1.ActiveRow.Cells["MATLOTNO"].Value);
            if (sLotNo != "")
            {
                ShowDialog("현재 투입된 LOT가 있습니다.\r\n투입 LOT를 취소 후 진행하세요.", DialogForm.DialogType.OK);
                return;
            }
            string sWorkcenterCode = Convert.ToString(grid1.ActiveRow.Cells["WORKCENTERCODE"].Value);
            string sWorkcenterName = Convert.ToString(grid1.ActiveRow.Cells["WORKCENTERNAME"].Value);

            // 팝업 호출
            POP_ORDERNO OrderPOP = new POP_ORDERNO(sWorkcenterCode, sWorkcenterName);
            OrderPOP.ShowDialog();

            if (OrderPOP.Tag == null) return;
            //ShowDialog(Convert.ToString(OrderPOP.Tag));
            // 작업장 현재 상태 테이블에 작업지시 등록 또는 업데이트
            DBHelper helper = new DBHelper("", true);
            try
            {
                string sPlantCode = Convert.ToString(grid1.ActiveRow.Cells["PLANTCODE"].Value);
                string sOrderNO = Convert.ToString(OrderPOP.Tag);
                helper.ExecuteNoneQuery("19PP_ActureOutput_I2", CommandType.StoredProcedure
                                    , helper.CreateParameter("PLANTCODE", sPlantCode, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("WORKCENTERCODE", sWorkcenterCode, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("WORKERID", sWorkerId, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("ORDERNO", sOrderNO, DbType.String, ParameterDirection.Input)
                                    );
                if (helper.RSCODE == "S")
                {
                    helper.Commit();
                    ShowDialog("작업지시 등록을 완료 하였습니다.");
                    cboWorkcenterCode_ValueChanged(null, null); // 작업지시 등록 내역 조회
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

        private void btnWorker_Click(object sender, EventArgs e)
        {
            // 작업자 등록 버튼 클릭 
            if (grid1.Rows.Count == 0) return;
            if (grid1.ActiveRow == null)
            {
                ShowDialog("작업장을 선택 후 진행 하세요.");
                return;
            }
            if (Convert.ToString(txtWorkerID.Text) == "")
            {
                ShowDialog("작업자를 선택 후 진행 하세요.");
                return;
            }

            string sWorkerId = Convert.ToString(txtWorkerID.Text); // 작업자 코드
            string sWorkcenterCode = Convert.ToString(
                                    grid1.ActiveRow.Cells["WORKCENTERCODE"].Value);
            string sPlantCode = Convert.ToString(
                                    grid1.ActiveRow.Cells["PLANTCODE"].Value);

            DBHelper helper = new DBHelper("", true);
            try
            {
                helper.ExecuteNoneQuery("19PP_ActureOutput_I1", CommandType.StoredProcedure
                                    , helper.CreateParameter("PLANTCODE", sPlantCode, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("WORKCENTERCODE", sWorkcenterCode, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("WORKERID", sWorkerId, DbType.String, ParameterDirection.Input)
                                    );
                if (helper.RSCODE == "S")
                {
                    helper.Commit();
                    ShowDialog("작업자 등록을 완료 하였습니다.");
                    cboWorkcenterCode_ValueChanged(null, null); // 작업자 등록 완료 후 재조회
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

        private void btnLotIn_Click(object sender, EventArgs e)
        {
            // LOT 투입 및 투입 취소
            string sPLANTCODE = Convert.ToString(grid1.ActiveRow.Cells["PLANTCODE"].Value);
            string sITEMCODE = Convert.ToString(grid1.ActiveRow.Cells["ITEMCODE"].Value);
            string sLOTNO = Convert.ToString(txtInLotNo.Text);
            string sWorkCenterCode = Convert.ToString(grid1.ActiveRow.Cells["WORKCENTERCODE"].Value);
            string sorderno = Convert.ToString(grid1.ActiveRow.Cells["ORDERNO"].Value);

            if (sorderno == "")
            {
                ShowDialog("등록된 작업지시가 없습니다.");
                return;
            }
            string sWORKERID = Convert.ToString(grid1.ActiveRow.Cells["WORKER"].Value);
            if (sWORKERID == "")
            {
                ShowDialog("등록된 작업자가 없습니다.");
                return;
            }
            string sUNITCODE = Convert.ToString(grid1.ActiveRow.Cells["UNITCODE"].Value);
            string sInFlag = Convert.ToString(btnLotIn.Text);
            if (sInFlag == "(4) LOT 투입")
            {
                sInFlag = "IN";
            }
            else sInFlag = "OUT";

            DBHelper helper = new DBHelper("", true);
            try
            {

                helper.ExecuteNoneQuery("09PP_ACTURLOUPUT_I3", CommandType.StoredProcedure
                                , helper.CreateParameter("PLANTCODE", sPLANTCODE, DbType.String, ParameterDirection.Input)
                                , helper.CreateParameter("WORKCENTERCODE", sWorkCenterCode, DbType.String, ParameterDirection.Input)
                                , helper.CreateParameter("ITEMCODE", sITEMCODE, DbType.String, ParameterDirection.Input)
                                , helper.CreateParameter("LOTNO", sLOTNO, DbType.String, ParameterDirection.Input)
                                , helper.CreateParameter("ORDERNO", sorderno, DbType.String, ParameterDirection.Input)
                                , helper.CreateParameter("UNITCODE", sUNITCODE, DbType.String, ParameterDirection.Input)
                                , helper.CreateParameter("INFLAG", sInFlag, DbType.String, ParameterDirection.Input)
                                , helper.CreateParameter("WORKERID", sWORKERID, DbType.String, ParameterDirection.Input)
                                );
                if (helper.RSCODE == "S")
                {
                    helper.Commit();
                    ShowDialog("LOT 투입/취소를 완료 하였습니다.");
                    cboWorkcenterCode_ValueChanged(null, null); //작업자지시 등록 내역 조회
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

        private void grid1_AfterRowActivate(object sender, EventArgs e)
        {
            // 작업장 현재 상태 내역을 컨트롤에 표시한다.
            if (Convert.ToString(this.grid1.ActiveRow.Cells["WORKSTATUSCODE"].Value) == "R")
            {
                btnRunStop.Text = "비가동";
            }
            else btnRunStop.Text = "가동";

            string sMatLotno = Convert.ToString(this.grid1.ActiveRow.Cells["MATLOTNO"].Value);
            if (sMatLotno != "")
            {
                txtInLotNo.Text = sMatLotno;
                btnLotIn.Text = "(4) LOT 투입 취소";
            }
            else
            {
                txtInLotNo.Text = "";
                btnLotIn.Text = "(4) LOT 투입";
            }
            txtWorkerID.Text = Convert.ToString(this.grid1.ActiveRow.Cells["WORKER"].Value);
            txtWorkerName.Text = Convert.ToString(this.grid1.ActiveRow.Cells["WORKERNAME"].Value);



        }

        private void btnRunStop_Click(object sender, EventArgs e)
        {
            if (grid1.ActiveRow == null)
            {
                ShowDialog("작업장을 등록해주세요.");
                return;
            }
            DBHelper helper = new DBHelper("", true);
            try
            {
                string sPlantCode = Convert.ToString(grid1.ActiveRow.Cells["PLANTCODE"].Value);
                string sStatus = "R";
                if (Convert.ToString(this.grid1.ActiveRow.Cells["WORKSTATUSCODE"].Value) == "R")
                {
                    sStatus = "S";
                    btnRunStop.Text = "비가동";
                }
                else btnRunStop.Text = "가동";
                string sWorkcenterCode = Convert.ToString(grid1.ActiveRow.Cells["WORKCENTERCODE"].Value);
                string sItemCode = Convert.ToString(grid1.ActiveRow.Cells["ITEMCODE"].Value);
                string sOrderNo = Convert.ToString(grid1.ActiveRow.Cells["ORDERNO"].Value);
                helper.ExecuteNoneQuery("19PP_ActureOutput_I4", CommandType.StoredProcedure
                                    , helper.CreateParameter("PLANTCODE", sPlantCode, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("WORKCENTERCODE", sWorkcenterCode, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("ITEMCODE", sItemCode, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("ORDERNO", sOrderNo, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("STATUS", sStatus, DbType.String, ParameterDirection.Input)
                                    );
                if (helper.RSCODE == "S")
                {
                    helper.Commit();
                    ShowDialog("LOT투입 취소 및 등록을 완료하였습니다.");
                    cboWorkcenterCode_ValueChanged(null, null); // LOT 투입/취소 후 데이터 조회
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
        private void btnProdIn_Click(object sender, EventArgs e)
        {
            // 생산 실적 등록 처리
            if (grid1.ActiveRow == null)
            {
                ShowDialog("작업장을 선택 후 진행하세요.");
                return;
            }
            string sPlnatCode = Convert.ToString(grid1.ActiveRow.Cells["PLANTCODE"].Value);
            double dProdQty = 0;     // 누적 양품 수량
            double dErrorQty = 0;    // 누적 불량품 수량
            double dTProdQty = 0;    // 입력한 양품 수량
            double dTBadQty = 0;     // 입력한 불량품 수량
            double dOrderQty = 0;    // 작업지시 수량

            string sProdQty = Convert.ToString(grid1.ActiveRow.Cells["PRODQTY"].Value).Replace(",", "");
            double.TryParse(sProdQty, out dProdQty);

            string sBadQty = Convert.ToString(grid1.ActiveRow.Cells["BADQTY"].Value).Replace(",", "");
            double.TryParse(sBadQty, out dErrorQty);

            string sTProdQty = Convert.ToString(txtProductQty.Text);
            double.TryParse(sTProdQty, out dTProdQty);

            string sTBadQty = Convert.ToString(txtBadQty.Text);
            double.TryParse(sTBadQty, out dTBadQty);

            string sTOrderQty = Convert.ToString(grid1.ActiveRow.Cells["ORDERQTY"].Value).Replace(",", "");
            double.TryParse(sTOrderQty, out dOrderQty);

            string sMatLotNo = Convert.ToString(grid1.ActiveRow.Cells["MATLOTNO"].Value);
            if (sMatLotNo == "")
            {
                ShowDialog("투입한 LOT가 존재하지 않습니다.");
            }
            if (dTProdQty + dTBadQty == 0)
            {
                ShowDialog("실적 수량을 입력하지 않았습니다.");
                return;
            }
            if (dOrderQty < dProdQty + dTProdQty)
            {
                ShowDialog("총 생산 수량이 지시 수량보다 많습니다.");
                return;
            }
            DBHelper helper = new DBHelper("", true);
            try
            {
                string sPlantCode = Convert.ToString(grid1.ActiveRow.Cells["PLANTCODE"].Value);
                string sWorkcenterCode = Convert.ToString(grid1.ActiveRow.Cells["WORKCENTERCODE"].Value);
                string sItemCode = Convert.ToString(grid1.ActiveRow.Cells["ITEMCODE"].Value);
                string sOrderNo = Convert.ToString(grid1.ActiveRow.Cells["ORDERNO"].Value);
                string sUnitCode = Convert.ToString(grid1.ActiveRow.Cells["UNITCODE"].Value);
                helper.ExecuteNoneQuery("99PP_ActualOutPut_I5", CommandType.StoredProcedure
                                    , helper.CreateParameter("PLANTCODE", sPlantCode, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("WORKCENTERCODE", sWorkcenterCode, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("ORDERNO", sOrderNo, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("ITEMCODE", sItemCode, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("UNITCODE", sUnitCode, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("PRODQTY", dTProdQty, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("BADQTY", dTBadQty, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("MATLOTNO", sMatLotNo, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("WORKER", LoginInfo.UserID, DbType.String, ParameterDirection.Input)
                                    );
                if (helper.RSCODE == "S")
                {
                    helper.Commit();
                    ShowDialog("생산 실적 등록을 완료하였습니다.");
                    cboWorkcenterCode_ValueChanged(null, null); // LOT 투입/취소 후 데이터 조회
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

        private void btnOrderClose_Click(object sender, EventArgs e)
        {
            // 작업지시 종료
            if (grid1.ActiveRow == null) return;
            // 투입 LOT가 있을 경우 종료 안되도록 함

            if (Convert.ToString(grid1.ActiveRow.Cells["MATLOTNO"].Value) != "")
            {
                ShowDialog("LOT 투입 취소 후 진행하세요.");
                return;
            }
            if (Convert.ToString(grid1.ActiveRow.Cells["WORKSTATUSCODE"].Value) == "R")
            {
                ShowDialog("비가동 등록 후 진행하세요.");
                return;
            }
            DBHelper helper = new DBHelper("", true);
            try
            {
                string sPlantCode = Convert.ToString(grid1.ActiveRow.Cells["PLANTCODE"].Value);
                string sWorkcenterCode = Convert.ToString(grid1.ActiveRow.Cells["WORKCENTERCODE"].Value);
                string sOrderNo = Convert.ToString(grid1.ActiveRow.Cells["ORDERNO"].Value);
                helper.ExecuteNoneQuery("19PP_ActureOutput_I6", CommandType.StoredProcedure
                                    , helper.CreateParameter("PLANTCODE", sPlantCode, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("WORKCENTERCODE", sWorkcenterCode, DbType.String, ParameterDirection.Input)
                                    , helper.CreateParameter("ORDERNO", sOrderNo, DbType.String, ParameterDirection.Input)
                                    );
                if (helper.RSCODE == "S")
                {
                    helper.Commit();
                    ShowDialog("작업 지시를 정상적으로 종료하였습니다.");
                    cboWorkcenterCode_ValueChanged(null, null); // LOT 투입/취소 후 데이터 조회
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
    }
}
