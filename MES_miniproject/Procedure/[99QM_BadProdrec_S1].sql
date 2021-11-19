USE [KFQB_MES_2021]
GO
/****** Object:  StoredProcedure [dbo].[99QM_BadProdrec_S1]    Script Date: 2021-07-16 오후 3:38:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		1조
-- Create date: 2021-07-11
-- Description:	불량 판정 이력 조회
-- =============================================
ALTER PROCEDURE [dbo].[99QM_BadProdrec_S1]
	 @PLANTCODE      VARCHAR(10) -- 공장
	,@WORKCENTERCODE VARCHAR(20) -- 작업장
	,@STARTDATE		 VARCHAR(10) -- 조회 시작일자
	,@ENDDATE		 VARCHAR(10) -- 조회 종료일자

	,@LANG	         VARCHAR(10)  = 'KO'
	,@RS_CODE        VARCHAR(1)   OUTPUT
	,@RS_MSG         VARCHAR(200) OUTPUT
	
AS
BEGIN
	SELECT PLANTCODE							AS PLANTCODE
		  ,WORKCENTERCODE						AS WORKCENTERCODE
		  ,WORKERID								AS WORKERID
		  ,ITEMCODE								AS ITEMCODE
		  ,DBO.FU_ITEMNAME(PLANTCODE, ITEMCODE) AS ITEMNAME
		  ,OUTCOME								AS OUTCOME
		  ,PRODQTY							    AS ProdQTY
		  ,BADQTY								AS BADQTY
		  ,CHKNO								AS CHKNO
		  ,REMARK								AS REMARK
		  ,MAKER								AS MAKER
		  ,MAKEDATE								AS MAKEDATE
	  FROM TB_BadProdQMrec WITH(NOLOCK) 
	  WHERE PLANTCODE      LIKE '%' + @PLANTCODE       + '%'
        AND WORKCENTERCODE LIKE '%' + @WORKCENTERCODE  + '%'
	    AND MAKEDATE       BETWEEN @STARTDATE + ' 00:00:00' AND @ENDDATE + ' 23:59:59'

		SET @RS_CODE = 'S'
	
END

