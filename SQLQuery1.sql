--Delete hamstercage where ID > 1;
--select h.Name, h.Gender, hc.Id, hc.HamsterCount from hamster h join hamstercage hc on hc.Id = h.HamsterCageId order by hc.Id

--select ActivitieCageid, Gender from hamster order by ActivitieCageid
--Alter table hamster ADD Countminutes INT;
--update hamster set ActivitieCageid = NULL where ActivitieCageid = 1;
--update hamstercage set Id = 1 where Id = 145
--delete AktivityLog where HamsterActivity = 2;
select * from hamster

--select h.Name, al.HamsterActivity, al.ActivityTime from hamster h
--join AktivityLog al
--on h.ID = al.HamsterID
--order by h.Name, al.ActivityTime

