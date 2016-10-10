@if "%SCM_TRACE_LEVEL%" NEQ "4" @echo off

:: ----------------------
:: KUDU Deployment Script
:: Version: 1.0.6
:: ----------------------

:: Prerequisites
:: -------------

:: Verify node.js installed
where node 2>nul >nul
IF %ERRORLEVEL% NEQ 0 (
  echo Missing node.js executable, please install node.js, if already installed make sure it can be reached from current environment.
  goto error
)

:: Setup
:: -----

setlocal enabledelayedexpansion

SET ARTIFACTS=%~dp0%..\artifacts

IF NOT DEFINED DEPLOYMENT_SOURCE (
  SET DEPLOYMENT_SOURCE=%~dp0%.
)

IF NOT DEFINED DEPLOYMENT_TARGET (
  SET DEPLOYMENT_TARGET=%ARTIFACTS%\wwwroot
)

IF NOT DEFINED NEXT_MANIFEST_PATH (
  SET NEXT_MANIFEST_PATH=%ARTIFACTS%\manifest

  IF NOT DEFINED PREVIOUS_MANIFEST_PATH (
    SET PREVIOUS_MANIFEST_PATH=%ARTIFACTS%\manifest
  )
)

IF NOT DEFINED KUDU_SYNC_CMD (
  :: Install kudu sync
  echo Installing Kudu Sync
  call npm install kudusync -g --silent
  IF !ERRORLEVEL! NEQ 0 goto error

  :: Locally just running "kuduSync" would also work
  SET KUDU_SYNC_CMD=%appdata%\npm\kuduSync.cmd
)
IF NOT DEFINED DEPLOYMENT_TEMP (
  SET DEPLOYMENT_TEMP=%temp%\___deployTemp%random%
  SET CLEAN_LOCAL_DEPLOYMENT_TEMP=true
)

IF DEFINED CLEAN_LOCAL_DEPLOYMENT_TEMP (
  IF EXIST "%DEPLOYMENT_TEMP%" rd /s /q "%DEPLOYMENT_TEMP%"
  mkdir "%DEPLOYMENT_TEMP%"
)

IF DEFINED MSBUILD_PATH goto MsbuildPathDefined
SET MSBUILD_PATH=%ProgramFiles(x86)%\MSBuild\14.0\Bin\MSBuild.exe
:MsbuildPathDefined

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
:: Deployment
:: ----------

echo Handling .NET Web Application deployment.

:: 1. Typings Install
IF NOT EXIST "%AppData%\npm\node_modules\typings\typings.json" (
  :: Install kudu sync
  echo Installing Typings
  call npm install typescript typings gulp-cli -g
  IF !ERRORLEVEL! NEQ 0 goto error

  :: Locally just running "kuduSync" would also work
  SET TYPINGS_CMD=%appdata%\npm\typings.cmd
)
:: 2. NPM Install
if EXIST "%DEPLOYMENT_SOURCE%\Promact.Oauth.Server\src\Promact.Oauth.Server\package.json" (    
    echo Installing NPM packages
	pushd "%DEPLOYMENT_SOURCE%\Promact.Oauth.Server\src\Promact.Oauth.Server"
    call :ExecuteCmd npm install --silent	
    IF !ERRORLEVEL! NEQ 0 goto error
    popd
)
if EXIST "%DEPLOYMENT_SOURCE%\Promact.Oauth.Server\src\Promact.Oauth.Server\tsconfig.json" (    
    echo Compiling Typescript
	pushd "%DEPLOYMENT_SOURCE%\Promact.Oauth.Server\src\Promact.Oauth.Server"
    call :ExecuteCmd tsc -p tsconfig.json	
    IF !ERRORLEVEL! NEQ 0 goto error
    popd
)
:: 2.5 Execute gulp task
if EXIST "%DEPLOYMENT_SOURCE%\Promact.Oauth.Server\src\Promact.Oauth.Server\package.json" (    
	echo Executing GULP Tasks
	pushd "%DEPLOYMENT_SOURCE%\Promact.Oauth.Server\src\Promact.Oauth.Server"
	call :ExecuteCmd gulp clean min bundle copytowwwroot
	IF !ERRORLEVEL! NEQ 0 goto error
    popd
}

:: 3. Bower Install
if EXIST "%DEPLOYMENT_SOURCE%\Promact.Oauth.Server\src\Promact.Oauth.Server\bower.json" (
    echo Executing bower install
	pushd "%DEPLOYMENT_SOURCE%\Promact.Oauth.Server\src\Promact.Oauth.Server"
    call :ExecuteCmd bower install
    IF !ERRORLEVEL! NEQ 0 goto error
    popd
)

:: 4. Restore nuget packages
call :ExecuteCmd nuget.exe restore "%DEPLOYMENT_SOURCE%\Promact.Oauth.Server\Promact.Oauth.Server.sln" -packagesavemode nuspec
IF !ERRORLEVEL! NEQ 0 goto error

:: 5. Build and publish
call :ExecuteCmd "%MSBUILD_PATH%" "%DEPLOYMENT_SOURCE%\Promact.Oauth.Server\Promact.Oauth.Server.sln" /nologo /verbosity:d /m /p:deployOnBuild=True;AutoParameterizationWebConfigConnectionStrings=false;Configuration=Release;UseSharedCompilation=false;publishUrl="%DEPLOYMENT_TEMP%" %SCM_BUILD_ARGS%
IF !ERRORLEVEL! NEQ 0 goto error

:: 6. KuduSync
call :ExecuteCmd "%KUDU_SYNC_CMD%" -v 50 -f "%DEPLOYMENT_TEMP%" -t "%DEPLOYMENT_TARGET%" -n "%NEXT_MANIFEST_PATH%" -p "%PREVIOUS_MANIFEST_PATH%" -i ".git;.hg;.deployment;deploy.cmd"
IF !ERRORLEVEL! NEQ 0 goto error

::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
goto end

:: Execute command routine that will echo out when error
:ExecuteCmd
setlocal
set _CMD_=%*
call %_CMD_%
if "%ERRORLEVEL%" NEQ "0" echo Failed exitCode=%ERRORLEVEL%, command=%_CMD_%
exit /b %ERRORLEVEL%

:error
endlocal
echo An error has occurred during web site deployment.
call :exitSetErrorLevel
call :exitFromFunction 2>nul

:exitSetErrorLevel
exit /b 1

:exitFromFunction
()

:end
endlocal
echo Finished successfully.
