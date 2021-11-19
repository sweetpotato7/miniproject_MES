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
    // 불량 판정 이력 조회
    public partial class QM_BadProdrec : DC00_WinForm.BaseMDIChildForm
    {

        private UltraGridUtil _GridUtil = new UltraGridUtil();
        DataTable rtnDtTemp = new DataTable();
        Common _Common = new Common();

        public QM_BadProdrec()
        {
            InitializeComponent();
        }

        private void QM_BadProdrec_Load(object sender, EventArgs e)
        {
            #region<그리트 셋팅>
            // 그리드 셋팅을 위한
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "공장", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, false, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, false, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKERID", "작업자", true, GridColDataType_emu.VarChar, 180, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품목", true, GridColDataType_emu.VarChar, 150, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품명", true, GridColDataType_emu.VarChar, 200, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "OutCome", "판정", true, GridColDataType_emu.VarChar, 200, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ProdQTY", "양품수량", true, GridColDataType_emu.Double, 100, 120, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "BADQTY", "불량수량", true, GridColDataType_emu.Double, 100, 120, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "CHKNO", "판정순번", true, GridColDataType_emu.VarChar, 100, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "REMARK", "사유", true, GridColDataType_emu.VarChar, 100, 120, Infragistics.Win.HAlign.Left, true, true);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKER", "생성자", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, false, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE", "생성일자", true, GridColDataType_emu.DateTime24, 150, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            // 공장
            rtnDtTemp = _Common.Standard_CODE("PLANTCODE");
            Common.FillComboboxMaster(this.cboPlantCode, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            // 작업장코드
            rtnDtTemp = _Common.GET_Workcenter_Code();
            Common.FillComboboxMaster(this.cboWorkcenterCode, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");


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
                dtTemp = helper.FillTable("99QM_BadProdrec_S1", CommandType.StoredProcedure,
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
    }
}
