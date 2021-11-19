USE [KFQB_MES_2021]
GO
/****** Object:  StoredProcedure [dbo].[99QM_BadProdAN_S1]    Script Date: 2021-07-16 오후 3:38:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		1조
-- Create date: 2021-07-12
-- Description:	불량 분석 데이터 조회
-- =============================================
ALTER PROCEDURE [dbo].[99QM_BadProdAN_S1]
	 @PLANTCODE      VARCHAR(10)  -- 공장
	,@WORKCENTERCODE VARCHAR(20)  -- 작업장
	,@OUTCOME        VARCHAR(10)  -- 판정
	,@REMARK	     VARCHAR(200) -- 사유
	,@STARTDATE		 VARCHAR(10)  -- 조회 시작일자
	,@ENDDATE		 VARCHAR(10)  -- 조회 종료일자

	,@LANG	         VARCHAR(10)  = 'KO'
	,@RS_CODE        VARCHAR(1)   OUTPUT
	,@RS_MSG         VARCHAR(200) OUTPUT
AS
BEGIN
	SELECT A.OUTCOME	                            AS OUTCOME
          ,A.REMARK                                 AS REMARK
          ,A.PLANTCODE                              AS PLANTCODE
		  ,A.WORKCENTERCODE                         AS WORKCENTERCODE
		  ,A.WORKERID                               AS WORKERID
		  ,A.ITEMCODE		                        AS ITEMCODE
		  ,DBO.FU_ITEMNAME(A.PLANTCODE, A.ITEMCODE) AS ITEMNAME
		  ,CONVERT(VARCHAR,A.MAKEDATE,23)		    AS MAKEDATE
		  ,A.BADQTY								    AS BADQTY
		  ,CONVERT(VARCHAR,A.CHKNO)                 AS CHKNO
		  ,A.MAKER								    AS MAKER
      FROM TB_BadProdQMrec A WITH(NOLOCK) INNER JOIN  (SELECT INDATESEQ, MAX(CHKNO) AS CHKNO
													     FROM TB_BadProdQMrec WITH(NOLOCK)
												     GROUP BY INDATESEQ) B
												  ON A.INDATESEQ = B.INDATESEQ
												 AND A.CHKNO     = B.CHKNO

	  WHERE A.PLANTCODE         LIKE '%' + @PLANTCODE       + '%'
        AND A.WORKCENTERCODE    LIKE '%' + @WORKCENTERCODE  + '%'
	    AND A.OUTCOME           LIKE '%' + @OUTCOME         + '%' 
	  	AND ISNULL(A.REMARK,'')	LIKE '%' + @REMARK          + '%' 
	    AND A.MAKEDATE          BETWEEN @STARTDATE + ' 00:00:00' AND @ENDDATE + ' 23:59:59'

   ORDER BY A.OUTCOME, A.REMARK, A.WORKERID

	   	SET @RS_CODE = 'S'
END

--SELECT * FROM TB_BadProdQMrec