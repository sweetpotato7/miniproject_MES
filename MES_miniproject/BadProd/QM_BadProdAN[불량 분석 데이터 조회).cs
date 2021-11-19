using System;
using System.Data;
using System.Drawing;
using System.Security;
using DC_POPUP;

using DC00_assm;
using DC00_WinForm;

using Infragistics.Win.UltraWinGrid;

namespace KFQB_Form
{
    public partial class QM_BadProdAN : DC00_WinForm.BaseMDIChildForm
    {

        private UltraGridUtil _GridUtil = new UltraGridUtil();
        DataTable rtnDtTemp = new DataTable();
        Common _Common = new Common();

        public QM_BadProdAN()
        {
            InitializeComponent();
        }

        private void QM_BadProdAN_Load(object sender, EventArgs e)
        {
            #region<그리트 셋팅>
            // 그리드 셋팅을 위한
            _GridUtil.InitializeGrid(this.grid1, false, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "OUTCOME", "판정", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "REMARK", "사유", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "공장", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKCENTERCODE", "작업장", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WORKERID", "작업자", true, GridColDataType_emu.VarChar, 180, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품목", true, GridColDataType_emu.VarChar, 150, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품명", true, GridColDataType_emu.VarChar, 200, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKEDATE", "생성일자", true, GridColDataType_emu.VarChar, 150, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "BADQTY", "불량수량", true, GridColDataType_emu.Double, 100, 120, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "CHKNO", "판정순번", true, GridColDataType_emu.VarChar, 100, 120, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "MAKER", "생성자", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.SetInitUltraGridBind(grid1);
            #endregion

            // 사업장
            rtnDtTemp = _Common.Standard_CODE("PLANTCODE");
            Common.FillComboboxMaster(this.cboPlantCode, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            UltraGridUtil.SetComboUltraGrid(this.grid1, "PLANTCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            // 작업장코드
            rtnDtTemp = _Common.GET_Workcenter_Code();
            Common.FillComboboxMaster(this.cboWorkcenterCode, rtnDtTemp, rtnDtTemp.Columns["CODE_ID"].ColumnName, rtnDtTemp.Columns["CODE_NAME"].ColumnName, "ALL", "");
            UltraGridUtil.SetComboUltraGrid(this.grid1, "WORKCENTERCODE", rtnDtTemp, "CODE_ID", "CODE_NAME");

            // merge
            this.grid1.DisplayLayout.Override.MergedCellContentArea = MergedCellContentArea.VirtualRect;
            this.grid1.DisplayLayout.Bands[0].Columns["OUTCOME"].MergedCellStyle = MergedCellStyle.Always;
            this.grid1.DisplayLayout.Bands[0].Columns["REMARK"].MergedCellStyle = MergedCellStyle.Always;
            this.grid1.DisplayLayout.Bands[0].Columns["PLANTCODE"].MergedCellStyle = MergedCellStyle.Always;
            this.grid1.DisplayLayout.Bands[0].Columns["WORKCENTERCODE"].MergedCellStyle = MergedCellStyle.Always;
            this.grid1.DisplayLayout.Bands[0].Columns["WORKERID"].MergedCellStyle = MergedCellStyle.Always;
            this.grid1.DisplayLayout.Bands[0].Columns["ITEMCODE"].MergedCellStyle = MergedCellStyle.Always;
            this.grid1.DisplayLayout.Bands[0].Columns["ITEMNAME"].MergedCellStyle = MergedCellStyle.Always;
            this.grid1.DisplayLayout.Bands[0].Columns["MAKEDATE"].MergedCellStyle = MergedCellStyle.Always;

        }

        public override void DoInquire()  //조회수행
        {
            #region ㅡㅡ 원본
            //// 조회 버튼 클릭
            //DBHelper helper = new DBHelper();
            //try
            //{  
            //    string sPlantCode        = Convert.ToString(cboPlantCode.Value);
            //    string sWorkCenterCode   = Convert.ToString(cboWorkcenterCode.Value);
            //    string sOutCome          = Convert.ToString(cboOutCome.Value);
            //    string sReMark           = Convert.ToString(cboRemark.Value);
            //    string sStartDate        = string.Format("{0:yyyy-MM-dd}", dtStart.Value);
            //    string sEndDate          = string.Format("{0:yyyy-MM-dd}", dtEnd.Value);

            //    DataTable dtTemp = new DataTable();
            //    dtTemp = helper.FillTable("99QM_BadProdAN_S1", CommandType.StoredProcedure,
            //                    helper.CreateParameter("PLANTCODE",      sPlantCode,      DbType.String, ParameterDirection.Input),
            //                    helper.CreateParameter("WORKCENTERCODE", sWorkCenterCode, DbType.String, ParameterDirection.Input),
            //                    helper.CreateParameter("OUTCOME",        sOutCome,        DbType.String, ParameterDirection.Input),
            //                    helper.CreateParameter("REMARK",         sReMark,         DbType.String, ParameterDirection.Input),
            //                    helper.CreateParameter("STARTDATE",      sStartDate,      DbType.String, ParameterDirection.Input),
            //                    helper.CreateParameter("ENDDATE",        sEndDate,        DbType.String, ParameterDirection.Input)
            //                    );
            //    if (dtTemp.Rows.Count == 0)
            //    {
            //        _GridUtil.Grid_Clear(grid1);  // 그리드의 내용을 초기화 한다.
            //        ShowDialog("조회할 데이터가 없습니다.", DC00_WinForm.DialogForm.DialogType.OK);
            //        return;
            //    }
            //    this.grid1.DataSource = dtTemp;
            //}
            //catch (Exception ex)
            //{

            //}
            //finally
            //{
            //    helper.Close();
            //}
            #endregion

            #region ㅡㅡ 추가
            // 조회 버튼 클릭
            DBHelper helper = new DBHelper(false);
            try
            {
                _GridUtil.Grid_Clear(grid1);
                string sPlantCode = Convert.ToString(cboPlantCode.Value);
                string sWorkCenterCode = Convert.ToString(cboWorkcenterCode.Value);
                string sOutCome = Convert.ToString(cboOutCome.Value);
                string sRemark = Convert.ToString(cboRemark.Value);
                string sStartDate = string.Format("{0:yyyy-MM-dd}", dtStart.Value);
                string sEndDate = string.Format("{0:yyyy-MM-dd}", dtEnd.Value);

                DataTable rtnDtTemp = new DataTable();
                rtnDtTemp = helper.FillTable("99QM_BadProdAN_S1", CommandType.StoredProcedure,
                                helper.CreateParameter("PLANTCODE", sPlantCode, DbType.String, ParameterDirection.Input),
                                helper.CreateParameter("WORKCENTERCODE", sWorkCenterCode, DbType.String, ParameterDirection.Input),
                                helper.CreateParameter("OUTCOME", sOutCome, DbType.String, ParameterDirection.Input),
                                helper.CreateParameter("REMARK", sRemark, DbType.String, ParameterDirection.Input),
                                helper.CreateParameter("STARTDATE", sStartDate, DbType.String, ParameterDirection.Input),
                                helper.CreateParameter("ENDDATE", sEndDate, DbType.String, ParameterDirection.Input)
                                );
                if (rtnDtTemp.Rows.Count != 0)
                {
                    // 데이터 테이블 서식 복사
                    DataTable dtSubTotal = rtnDtTemp.Clone();

                    string sWorkerID = Convert.ToString(rtnDtTemp.Rows[0]["WORKERID"]);
                    double sBadQty = Convert.ToDouble(rtnDtTemp.Rows[0]["BADQTY"]);

                    dtSubTotal.Rows.Add(new object[]
                    {
                        Convert.ToString(rtnDtTemp.Rows[0]["OUTCOME"]),
                        Convert.ToString(rtnDtTemp.Rows[0]["REMARK"]),
                        Convert.ToString(rtnDtTemp.Rows[0]["PLANTCODE"]),
                        Convert.ToString(rtnDtTemp.Rows[0]["WORKCENTERCODE"]),
                        Convert.ToString(rtnDtTemp.Rows[0]["WORKERID"]),
                        Convert.ToString(rtnDtTemp.Rows[0]["ITEMCODE"]),
                        Convert.ToString(rtnDtTemp.Rows[0]["ITEMNAME"]),
                        Convert.ToString(rtnDtTemp.Rows[0]["MAKEDATE"]),
                        Convert.ToDouble(rtnDtTemp.Rows[0]["BADQTY"]),
                        Convert.ToString(rtnDtTemp.Rows[0]["CHKNO"]),
                        Convert.ToString(rtnDtTemp.Rows[0]["MAKER"])
                    });

                    for (int i = 1; i < rtnDtTemp.Rows.Count; i++)
                    {
                        if (sWorkerID == Convert.ToString(rtnDtTemp.Rows[i]["WORKERID"]))
                        {
                            sBadQty = sBadQty + Convert.ToDouble(rtnDtTemp.Rows[i]["BADQTY"]);
                            dtSubTotal.Rows.Add(new object[]
                            {
                                Convert.ToString(rtnDtTemp.Rows[i]["OUTCOME"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["REMARK"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["PLANTCODE"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["WORKCENTERCODE"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["WORKERID"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["ITEMCODE"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["ITEMNAME"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["MAKEDATE"]),
                                Convert.ToDouble(rtnDtTemp.Rows[i]["BADQTY"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["CHKNO"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["MAKER"])
                            });
                        }
                        else
                        {
                            dtSubTotal.Rows.Add(new object[] { "", "", "", "", "", "", "", "합계 :", sBadQty, "", "" });
                            sBadQty = Convert.ToDouble(rtnDtTemp.Rows[i]["BADQTY"]);
                            dtSubTotal.Rows.Add(new object[]
                            {
                                Convert.ToString(rtnDtTemp.Rows[i]["OUTCOME"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["REMARK"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["PLANTCODE"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["WORKCENTERCODE"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["WORKERID"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["ITEMCODE"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["ITEMNAME"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["MAKEDATE"]),
                                Convert.ToDouble(rtnDtTemp.Rows[i]["BADQTY"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["CHKNO"]),
                                Convert.ToString(rtnDtTemp.Rows[i]["MAKER"])
                            });
                            sWorkerID = Convert.ToString(rtnDtTemp.Rows[i]["WORKERID"]);
                        }
                    }
                    dtSubTotal.Rows.Add(new object[] { "", "", "", "", "", "", "", "합계 :", sBadQty, "", "" });


                    this.grid1.DataSource = dtSubTotal;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                helper.Close();
            }
            #endregion

        }

        // 사유별 병합
        private void grid1_InitializeLayout(object sender, InitializeLayoutEventArgs e)
        {
            CustomMergedCellEvalutor CM1 = new CustomMergedCellEvalutor("OUTCOME", "REMARK");
            e.Layout.Bands[0].Columns["PLANTCODE"].MergedCellEvaluator = CM1;
            e.Layout.Bands[0].Columns["WORKCENTERCODE"].MergedCellEvaluator = CM1;
            e.Layout.Bands[0].Columns["WORKERID"].MergedCellEvaluator = CM1;
            e.Layout.Bands[0].Columns["ITEMCODE"].MergedCellEvaluator = CM1;
            e.Layout.Bands[0].Columns["ITEMNAME"].MergedCellEvaluator = CM1;
            e.Layout.Bands[0].Columns["MAKEDATE"].MergedCellEvaluator = CM1;
        }
    }
}
