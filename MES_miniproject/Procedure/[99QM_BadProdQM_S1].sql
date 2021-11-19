USE [KFQB_MES_2021]
GO
/****** Object:  StoredProcedure [dbo].[99QM_BadProdQM_S1]    Script Date: 2021-07-16 오후 3:38:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		1조
-- Create date: 2021-07-09
-- Description:	불량 내역 조회 및 재검사 조회
-- =============================================
ALTER PROCEDURE [dbo].[99QM_BadProdQM_S1]
	 @PLANTCODE      VARCHAR(10) -- 공장
	,@WORKCENTERCODE VARCHAR(20) -- 작업장
	,@STARTDATE		 VARCHAR(10) -- 조회 시작일자
	,@ENDDATE		 VARCHAR(10) -- 조회 종료일자

	,@LANG	         VARCHAR(10)  = 'KO'
	,@RS_CODE        VARCHAR(1)   OUTPUT
	,@RS_MSG         VARCHAR(200) OUTPUT
AS
BEGIN
	SELECT CASE WHEN ISNULL(A.DELFLAG,'N') = 'Y' THEN 1
		         ELSE 0 END	                            AS DELFLAG
          ,A.REMARK                                     AS REMARK
          ,A.PLANTCODE                                  AS PLANTCODE
		  ,A.WORKCENTERCODE                             AS WORKCENTERCODE
		  ,A.WORKERID                                   AS WORKERID
		  ,A.ITEMCODE		                            AS ITEMCODE
		  ,DBO.FU_ITEMNAME(A.PLANTCODE, A.ITEMCODE)     AS ITEMNAME
		  ,A.FIRSTBADQTY							    AS FIRSTBADQTY
		  ,A.ORDERNO							        AS ORDERNO
		  ,A.BADQTY									    AS BADQTY
		  ,A.CHKNO									    AS CHKNO
		  ,A.MAKER									    AS MAKER
		  ,A.MAKEDATE								    AS MAKEDATE
		  ,A.INDATESEQ								    AS INDATESEQ
		  ,A.LOTNO									    AS LOTNO
      FROM TB_BadProdQM A WITH(NOLOCK)
     WHERE A.PLANTCODE             LIKE '%' + @PLANTCODE       + '%'
       AND A.WORKCENTERCODE        LIKE '%' + @WORKCENTERCODE  + '%'
	   AND A.MAKEDATE              BETWEEN @STARTDATE + ' 00:00:00' AND @ENDDATE + ' 23:59:59'

	SET @RS_CODE = 'S'
END


