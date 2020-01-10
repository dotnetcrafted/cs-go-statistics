import React, { ReactNode } from 'react';
import {
    List, Avatar, Typography
} from 'antd';

const { Title } = Typography;
const URL_REP = 'https://api.github.com/repos/dotnetcrafted/cs-go-statistics/contributors';

export default class AuthorsCopyright extends React.Component {
    state = {
        data: [
            {
                avatar_url: '',
                html_url: '',
                login: ''
            }
        ]
    };

    private getGithubAccounts = async () => {
        const accounts = await fetch(URL_REP)
            .then((res: Response) => res.json())
            .then((acc) => acc.filter((elem: { type: string }) => elem.type === 'User'))
            .catch((err) => {
                throw new Error(err);
            });

        this.setState({
            data: accounts
        });
    };

    componentDidMount = () => {
        this.getGithubAccounts();
    };

    render() {
        const { data } = this.state;
        return (
            <List
                size="small"
                bordered={true}
                header={
                    <Title level={4}>Authors and Contributors:</Title>
                }
                itemLayout="horizontal"
                grid={{
                    gutter: 10,
                    xl: 3,
                    sm: 2,
                    xs: 1
                }}
                dataSource={data}
                renderItem={(person): ReactNode => (
                    <List.Item style={{ marginTop: '10px' }}>
                        <List.Item.Meta
                            avatar={
                                <Avatar icon="user" src={person.avatar_url} />
                            }
                            title={
                                <div>
                                    <a href={person.html_url}>{person.login}</a>
                                </div>
                            }
                        />
                    </List.Item>
                )}
            />
        );
    }
}
