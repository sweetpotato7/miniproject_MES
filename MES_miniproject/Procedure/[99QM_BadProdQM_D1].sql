USE [KFQB_MES_2021]
GO
/****** Object:  StoredProcedure [dbo].[99QM_BadProdQM_D1]    Script Date: 2021-07-16 오후 3:38:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		1조
-- Create date: 2021-07-12
-- Description:	불량 내역 조회 및 재검사 등록 - 폐기
-- =============================================
ALTER PROCEDURE [dbo].[99QM_BadProdQM_D1]
	 @PLANTCODE      VARCHAR(10)  -- 공장			
	,@INDATESEQ	     VARCHAR(20)  -- INDATESEQ	
	,@REMARK	     VARCHAR(200) -- 사유		
	,@ORDERNO        VARCHAR(20)  -- ORDERNO	
	,@WORKCENTERCODE VARCHAR(20)  -- 작업장 코드
	,@ITEMCODE		 VARCHAR(20)  -- 품목		
	,@LOTNO          VARCHAR(30)  -- LOTNO		
	,@WORKERID       VARCHAR(20)  -- 작업자		 
	,@BADQTY		 FLOAT		  -- 불량수량
	,@FIRSTBADQTY    FLOAT        -- 초기 불량 수량

	,@LANG	         VARCHAR(10)  = 'KO'
	,@RS_CODE        VARCHAR(1)   OUTPUT
	,@RS_MSG         VARCHAR(200) OUTPUT
AS
BEGIN
-- 현재시간 정의공통 변수
	DECLARE @LD_NOWDATE DATETIME
	       ,@LS_NOWDATE VARCHAR(10)
	    SET @LD_NOWDATE = GETDATE()
		SET @LS_NOWDATE = CONVERT(VARCHAR, @LD_NOWDATE, 23)
-- 폐기 시
	BEGIN
	-- TB_BadProdQM에서 삭제
		DELETE TB_BadProdQM
		 WHERE PLANTCODE = @PLANTCODE
		   AND INDATESEQ = @INDATESEQ

	-- 불량 이력 검사 횟수 채번
	DECLARE @LI_CHKNO INT
     SELECT @LI_CHKNO = ISNULL(MAX(CHKNO),0) + 1
	   FROM TB_BadProdQMrec WITH(NOLOCK)
	  WHERE PLANTCODE = @PLANTCODE
		AND INDATESEQ = @INDATESEQ


	-- 불량 이력 추가
		INSERT INTO TB_BadProdQMrec
			   ( PLANTCODE,       INDATESEQ,     LOTNO,     ORDERNO,  REMARK,   
				 WORKCENTERCODE,  WORKERID,      ITEMCODE,  OUTCOME,  FIRSTBADQTY,  
				 BADQTY,          CHKNO,         MAKER,     MAKEDATE)
		VALUES ( @PLANTCODE,      @INDATESEQ,    @LOTNO,    @ORDERNO, @REMARK,  
				 @WORKCENTERCODE, @WORKERID,     @ITEMCODE, '폐기',   @FIRSTBADQTY,
				 @BADQTY,         @LI_CHKNO,     @WORKERID, @LD_NOWDATE)
	END
END
