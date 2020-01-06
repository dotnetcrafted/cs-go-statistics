import React from 'react';
import { List, Avatar, Tag, Typography } from 'antd';

const { Title } = Typography;


export default class AuthorsCopyright extends React.Component {

    state ={
        data: [{
            avatar_url: '',
            url: '',
            login: ''
        }]
    }

    private gettingGithubAccounts = async () => {
        const accounts = await 
            fetch(`https://api.github.com/repos/dotnetcrafted/cs-go-statistics/contributors`)
                .then((res: Response) => res.json())
                .then(acc => acc.filter((elem: { type: string; }) => elem.type == "User"))
                .catch(err => {
                        throw new Error(err)
                })

        this.setState({
            data: accounts
        })
    }

    componentWillMount = () => {
        this.gettingGithubAccounts()
    }

    
    render() {
        const data = this.state.data
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
                                    <a href={person.url}>{person.login}</a> – <Tag>Developer</Tag>
                                </div>
                            }
                        />
                    </List.Item>
                )}
            />
        )
    }
}
