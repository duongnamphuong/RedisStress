use RedisStress
declare @Max int=100000
declare @i int=1
while @i<=@Max
begin
	--leading zeros, length of 20 characters
	declare @Imei nvarchar(20) = RIGHT('00000000000000000000'+CAST(@i AS VARCHAR(20)),20)
	insert into Product (Imei,[State],LastHbUtc) values(@Imei,1,null)
	set @i=@i+1
end