import React, { ReactNode } from 'react';

const URL_REP = 'https://api.github.com/repos/dotnetcrafted/cs-go-statistics/contributors';

export default class AuthorsCopyright extends React.Component<any, AuthorsCopyrightState> {
    state = {
        data: [
            {
                /* eslint-disable */
                avatar_url: '',
                html_url: '',
                login: ''
                /* eslint-enable */
            }
        ]
    };

    private getGithubAccounts = async () => {
        const accounts: UserData[] = await fetch(URL_REP)
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

    render(): ReactNode {
        const { data } = this.state;
        return (
            <div className="contributors">
                <div className="contributors__title">
                    <h4 className="contributors__title_txt">Authors and Contributors</h4>
                </div>

                <div className="contributors__content">
                    <ul className="contributors__list">
                        {data.map((item, index) => (
                            <li key={index} className="contributors__item">
                                <a className="contributors__link" href={item.html_url}>

                                    <div className="contributors__item_title">
                                        {item.login}
                                    </div>
                                </a>
                            </li>
                        ))}
                    </ul>
                </div>
            </div>
        );
    }
}

type AuthorsCopyrightState = {
    data: UserData[];
};

type UserData = {
    avatar_url: string;
    html_url: string;
    login: string;
};
