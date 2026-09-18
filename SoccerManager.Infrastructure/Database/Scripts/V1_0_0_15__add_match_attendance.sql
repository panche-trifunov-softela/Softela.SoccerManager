-- V1_0_0_13 is already applied and is never edited; Evolve rejects a changed checksum.
-- Attendance is NOT NULL with a default of 0 because ALTER TABLE ... ADD of a NOT NULL column fails on a table that already has rows; 0 also means "not played yet", so the default doubles as the value a freshly scheduled match carries.
-- CK_Matches_AttendanceNonNegative mirrors the validators' GreaterThanOrEqualTo(0), so the two agree.
ALTER TABLE dbo.Matches ADD Attendance INT NOT NULL CONSTRAINT DF_Matches_Attendance DEFAULT 0;

ALTER TABLE dbo.Matches ADD CONSTRAINT CK_Matches_AttendanceNonNegative CHECK (Attendance >= 0);
