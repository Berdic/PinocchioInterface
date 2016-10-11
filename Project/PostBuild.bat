REM %1 = Solution Directory
REM %2 = $(ConfigurationName) Debug/Release
REM %3 = $(PlatformName) x86/x64


IF  [%3] == x86 (

	ECHO Using x86 configuration settings 
	
	MD "%1PinocchioInterface\bin\%2\Pinocchio"
	xcopy "%1Resources\Pinocchio\" "%1PinocchioInterface\bin\%2\Pinocchio" /Y /E /R /I /D
	xcopy "%1Resources\Pinocchio\*.*" "%1PinocchioInterface\bin\%2\Pinocchio" /Y /E
	
		ECHO Application x86 folder
		
		MD "%1..\Application %3"
		xcopy "%1PinocchioInterface\bin\%3\%2\" "%1..\Application %3\"/Y /E /R /I /D
		xcopy "%1PinocchioInterface\bin\%3\%2\*.*" "%1.\Application %3\"/Y /E
	
) ELSE (

	ECHO Using x64 configuration settings DOOBIA
	
	MD "%1PinocchioInterface\bin\%3\%2\Pinocchio"
	xcopy "%1Resources\Pinocchio\" "%1PinocchioInterface\bin\%3\%2\Pinocchio\" /Y /E /R /I /D
	xcopy "%1Resources\Pinocchio\*.*" "%1PinocchioInterface\bin\%3\%2\Pinocchio\" /Y /E
	
		ECHO Application x64 folder
		
		MD "%1..\Application %3"
		xcopy "%1PinocchioInterface\bin\%3\%2\" "%1..\Application %3\"/Y /E /R /I /D
		xcopy "%1PinocchioInterface\bin\%3\%2\*.*" "%1..\Application %3\"/Y /E
)
