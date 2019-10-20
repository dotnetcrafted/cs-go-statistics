pipeline {
  agent any
 
  tools {nodejs "NodeJSLocal"}
 
  stages {
    stage('Git Checkout') {
        steps {
            git branch: 'Live', url: 'https://bitbucket.org/radik_fayskhanov/counterstrikestat.git'
        }
    }
    stage('Frontned') {
      steps {
        bat 'yarn --cwd ./CsStat.Web/ install'
        bat 'yarn --cwd ./CsStat.Web/ run prod'
      }
    }
    stage('Backend') {
      steps {
          bat label: '', script: '"C:\\tools\\nuget.exe" restore ./CsStat.Web/CsStat.Web.csproj -SolutionDirectory ./'
          bat "\"${tool 'MSBuildLocal'}\" ./CsStat.Web/CsStat.Web.csproj /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"
      }
    }
  }
}