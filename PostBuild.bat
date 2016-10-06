REM %1 = Solution Directory
REM %2 = $(ConfigurationName) Debug/Release
REM %3 = $(PlatformName) x86/x64

IF  [%3] == x86 (

	ECHO Using x86 configuration settings 
	
	ECHO Copying Pinocchio subdirectory to build destination
	
	MD "%1PinocchioInterface\bin\%2\Pinocchio"
	xcopy "%1Resources\Pinocchio\" "%1PinocchioInterface\bin\%2\Pinocchio" /Y /E /R /I /D
	xcopy "%1Resources\Pinocchio\*.*" "%1PinocchioInterface\bin\%2\Pinocchio" /Y /E
	
	ECHO Copying builded x86 version to Application x86 subdirectory
	
	MD "%1Application %3"
	xcopy "%1PinocchioInterface\bin\%2\" "%1Application %3\"/Y /E /R /I /D
	xcopy "%1PinocchioInterface\bin\%2\*.*" "%1Application %3\" /Y /E
	
) ELSE (

	ECHO Using x64 configuration settings DOOBIA
	
	ECHO Copying Pinocchio subdirectory to build destination
	
	MD "%1PinocchioInterface\bin\%3\%2\Pinocchio"
	xcopy "%1Resources\Pinocchio\" "%1PinocchioInterface\bin\%3\%2\Pinocchio\" /Y /E /R /I /D
	xcopy "%1Resources\Pinocchio\*.*" "%1PinocchioInterface\bin\%3\%2\Pinocchio\" /Y /E
	
	ECHO Copying builded x64 version to Application x64 subdirectory
	
	MD "%1Application %3"
	xcopy "%1PinocchioInterface\bin\%3\%2\" "%1Application %3\"/Y /E /R /I /D
	xcopy "%1PinocchioInterface\bin\%3\%2\*.*" "%1Application %3\" /Y /E
	
)
