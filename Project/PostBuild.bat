REM %1 = Solution Directory
REM %2 = $(ConfigurationName) Debug/Release
REM %3 = $(PlatformName) x86/x64


IF  [%3] == "x86" (

	ECHO Using x86 configuration settings 
	
	MD "%1PinocchioInterface\bin\%2\Pinocchio"
	xcopy "%1Resources\Pinocchio\" "%1PinocchioInterface\bin\%2\Pinocchio" /Y /E /R /I /D
	xcopy "%1Resources\Pinocchio\*.*" "%1PinocchioInterface\bin\%2\Pinocchio" /Y /E
	
	xcopy "%1Pinocchio\Pinocchio.dll" "%1PinocchioInterface\bin\%2" /Y /E
	
	
	
) ELSE (

	ECHO Using x64 configuration settings DOOBIA
	
	MD "%1PinocchioInterface\bin\%3\%2\Pinocchio"
	xcopy "%1Resources\Pinocchio\" "%1PinocchioInterface\bin\%3\%2\Pinocchio\" /Y /E /R /I /D
	xcopy "%1Resources\Pinocchio\*.*" "%1PinocchioInterface\bin\%3\%2\Pinocchio\" /Y /E
	
	
	ECHO COPY FROM %1Pinocchio\Pinocchio.dll TO %1PinocchioInterface\bin\%3\%2
	
	xcopy "%1\%2\Pinocchio.dll" "%1PinocchioInterface\bin\%3\%2" /Y /E
	
	
	
)
