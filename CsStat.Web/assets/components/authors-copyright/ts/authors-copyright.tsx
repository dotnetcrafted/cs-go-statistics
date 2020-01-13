import React from 'react';
import { List, Avatar, Tag, Typography } from 'antd';

const { Title } = Typography;


export default class AuthorsCopyright extends React.Component {

    state ={
        data: [],
        role: []
    }

    gettingGithubAccounts = async () => {
        const api_url = await 
            fetch(`https://api.github.com/repos/dotnetcrafted/cs-go-statistics/contributors`)

        const accounts = await api_url.json()

        this.setState({
            data: accounts.filter(elem => elem.type == "User")
        })

        this.state.data.forEach(item => {
            this.gettingRole(item)
        })
    }

    gettingRole = async (item) => {

        const javaScriptCounter = 0
        const cCounter = 0

        const api_repos_url = await 
            fetch(`${item.repos_url}`)

        const repos = await api_repos_url.json()

        repos.forEach(item => {
            item.language == "JavaScript" ? javaScriptCounter = javaScriptCounter + 1 : null
            item.language == "C#" ? cCounter = cCounter + 1 : null

        });

        console.log(item.login, "JavaScript =" + javaScriptCounter, "C# =" + cCounter)

    }

    componentWillMount = () => {
        this.gettingGithubAccounts()
    }

    
    render() {
        const data = this.state.data != [] ? this.state.data : 'asdasd'
        return (
            <List
                size="small"
                bordered={true}
                header={<Title level={4}>Authors and Contributors:</Title>}
                itemLayout="horizontal"
                dataSource={data}
                renderItem={person => (
                    <List.Item>
                        <List.Item.Meta
                            avatar={<Avatar src={person.avatar_url} />}
                            title={
                                <div>
                                    <a href={person.url}>{person.login}</a> – <Tag>{person.role}</Tag>
                                </div>
                            }
                        />
                    </List.Item>
                )}
            />
        )
    }
}


/*const githubAccounts = {
    rdk174: 'Rdk174',
    medprj: 'Medprj',
    host: 'hostile-d'
};

const data = [
    {
        name: 'Radik F',
        githubLink: `https://github.com/${githubAccounts.rdk174}`,
        avatarSrc: `https://github.com/${githubAccounts.rdk174}.png`,
        role: 'backend'
    },
    {
        name: 'Alexander M',
        githubLink: `https://github.com/${githubAccounts.medprj}`,
        avatarSrc: `https://github.com/${githubAccounts.medprj}.png`,
        role: 'backend'
    },
    {
        name: 'Danil S',
        githubLink: `https://github.com/${githubAccounts.host}`,
        avatarSrc: `https://github.com/${githubAccounts.host}.png`,
        role: 'frontend'
    }
];*/
