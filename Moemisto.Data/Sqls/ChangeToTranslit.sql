/****** Object:  UserDefinedFunction [dbo].[ChangeToTranslit]    Script Date: 7/27/2015 3:01:16 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Oleksandr Tkachuk

-- Description:	Transliteration
-- =============================================
CREATE FUNCTION [dbo].[ChangeToTranslit] 
(
	@ukrainianPhrase nvarchar(150)
)
RETURNS nvarchar(150)
AS
BEGIN
	Select @ukrainianPhrase = Replace(@ukrainianPhrase, Ukr, Latin) From Translit

	Return @ukrainianPhrase;

END

GO


