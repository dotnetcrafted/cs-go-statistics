import React, { ReactNode } from 'react';
import {
    Table, Avatar, Divider, Tooltip, Dropdown, Icon, Button
} from 'antd';
import { connect } from 'react-redux';
import { ColumnProps } from 'antd/es/table';
import qs from 'query-string';
import {
    fetchPlayers, startRequest, stopRequest
} from '../../../general/ts/redux/actions';
import FilterForm, { DateValues } from './filter-form';
import { IAppState, RootState, Player } from '../../../general/ts/redux/types';
import ColumnsSelector from './columns-selector';
import { nameof } from '../../../general/ts/extentions';
import '../scss/index.scss';
import utils from '../../../general/ts/utils';
import { history } from '../../../general/ts/redux/store';
import { DEFAULT_CMS_PLAYER, getPlayerById } from '../../../project/helpers';

const CELL_CSS_CLASS = 'players-data__cell';
const HIDDEN_CELL_CSS_CLASS = 'is-hidden';

export const COLUMN_NAMES: ColumnNames = {
    imagePath: { dataIndex: nameof<Player>('steamImage'), readableName: 'Steam Image' },
    name: { dataIndex: nameof<Player>('name'), readableName: 'Players Name' },
    points: { dataIndex: nameof<Player>('points'), readableName: 'Points' },
    kad: { dataIndex: nameof<Player>('kad'), readableName: 'K/A/D' },
    kdRatio: { dataIndex: nameof<Player>('kdRatio'), readableName: 'KD Ratio' },
    kdDif: { dataIndex: nameof<Player>('kdDif'), readableName: 'KD Diff' },
    headShot: { dataIndex: nameof<Player>('headShot'), readableName: 'HeadShots' },
    totalGames: { dataIndex: nameof<Player>('totalGames'), readableName: 'Total Games' },
    kills: { dataIndex: nameof<Player>('kills'), readableName: 'Kills' },
    assists: { dataIndex: nameof<Player>('assists'), readableName: 'Assists' },
    deaths: { dataIndex: nameof<Player>('deaths'), readableName: 'Deaths' },
    killsPerGame: { dataIndex: nameof<Player>('killsPerGame'), readableName: 'Kills/Game' },
    assistsPerGame: { dataIndex: nameof<Player>('assistsPerGame'), readableName: 'Assists/Game' },
    defusedBombs: { dataIndex: nameof<Player>('defusedBombs'), readableName: 'Defused Bombs' },
    explodedBombs: { dataIndex: nameof<Player>('explodedBombs'), readableName: 'Exploded Bombs' },
    friendlyKills: { dataIndex: nameof<Player>('friendlyKills'), readableName: 'Friendly Kills' },
};
const DEFAULT_COLUMNS = [
    COLUMN_NAMES.points.dataIndex,
    COLUMN_NAMES.kad.dataIndex,
    COLUMN_NAMES.kdRatio.dataIndex,
    COLUMN_NAMES.kdDif.dataIndex,
    COLUMN_NAMES.headShot.dataIndex,
    COLUMN_NAMES.totalGames.dataIndex,
];

const PERMANENT_COLUMNS = [COLUMN_NAMES.imagePath.dataIndex, COLUMN_NAMES.name.dataIndex];

class PlayersData extends React.Component<PlayersDataProps, PlayersDataState> {
    readonly state = {
        visibleColumns: [...DEFAULT_COLUMNS, ...PERMANENT_COLUMNS]
    };

    private fetchPlayers(playersDataUrl: string, params?: DateValues): void {
        const url = new URL(playersDataUrl, window.location.origin);
        if (params) {
            url.search = new URLSearchParams(params).toString();
        }

        this.props.startRequest();

        fetch(url.toString())
            .then((res: Response) => res.json())
            .then((data: IAppState) => {
                data = typeof data === 'string' ? JSON.parse(data) : data;
                this.props.fetchPlayers(data);
            })
            .catch((error) => {
                this.props.stopRequest();
                throw new Error(error);
            });
    }

    private getAvatar(record: Player): ReactNode {
        if (record.steamImage) {
            return <Avatar className="players-data__avatar" src={record.steamImage} />;
        }
        return <Avatar icon="user" />;
    }

    private onRowClick(record: Player): void {
        const search = utils.getUrlSearch({ playerId: record.id }, this.props.router.location.search);
        history.push({
            search
        });
    }

    private cellWrapper(id: string, content: ReactNode, value?: number): ReactNode {
        const search = qs.parse(this.props.router.location.search);
        const { playerId } = search;
        const isSelectedClass = id === playerId ? 'is-selected' : '';
        let textColorClass = '';

        if (value) {
            textColorClass = value < 0 ? 'red-text' : 'green-text';
        }

        return <div className={`players-data__cell-inner ${isSelectedClass} ${textColorClass}`}>{content}</div>;
    }

    onFormSubmit = (params: DateValues): void => {
        this.fetchPlayers(this.props.playersDataUrl, params);

        const search = utils.getUrlSearch(params, this.props.router.location.search);
        history.push({
            search
        });
    };

    onCheckboxesChange = (selectedColumns: string[]): void => {
        const visibleColumns = [...PERMANENT_COLUMNS, ...selectedColumns];
        this.setState({ visibleColumns });
    };

    private getCellClassName(dataIndex: string): string {
        if (this.state.visibleColumns.includes(dataIndex)) {
            return CELL_CSS_CLASS;
        }
        return `${CELL_CSS_CLASS} ${HIDDEN_CELL_CSS_CLASS}`;
    }

    private getColumns = (): ColumnProps<Player>[] => [
        {
            dataIndex: COLUMN_NAMES.imagePath.dataIndex,
            render: (_link: any, record: Player) => {
                const content = this.getAvatar(record);
                return this.cellWrapper(record.id, content);
            },
            width: '5%',
            className: this.getCellClassName(COLUMN_NAMES.imagePath.dataIndex)
        },
        {
            dataIndex: COLUMN_NAMES.name.dataIndex,
            title: COLUMN_NAMES.name.readableName,
            className: this.getCellClassName(COLUMN_NAMES.name.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, record.name),
            sorter: (a: Player, b: Player) => a.name.localeCompare(b.name)
        },
        {
            dataIndex: COLUMN_NAMES.points.dataIndex,
            title: COLUMN_NAMES.points.readableName,
            className: this.getCellClassName(COLUMN_NAMES.points.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, record.points),
            sorter: (a: Player, b: Player) => b.points - a.points
        }, // points
        {
            dataIndex: COLUMN_NAMES.kad.dataIndex,
            title: () => <Tooltip title="Kills / Assists / Deaths">{COLUMN_NAMES.kad.readableName}</Tooltip>,
            className: this.getCellClassName(COLUMN_NAMES.kad.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, record.kad),
            sorter: (a: Player, b: Player) => a.kills - b.kills
        }, // kad
        {
            dataIndex: COLUMN_NAMES.kdRatio.dataIndex,
            title: () => <Tooltip title="Kills / Deaths">{COLUMN_NAMES.kdRatio.readableName}</Tooltip>,
            className: this.getCellClassName(COLUMN_NAMES.kdRatio.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, record.kdRatio),
            sorter: (a: Player, b: Player) => b.kdRatio - a.kdRatio
        }, // Kd ratio
        {
            dataIndex: COLUMN_NAMES.kdDif.dataIndex,
            title: COLUMN_NAMES.kdDif.readableName,
            className: this.getCellClassName(COLUMN_NAMES.kdDif.dataIndex),
            render: (_link: any, record: Player) =>
                this.cellWrapper(record.id, record.kdDif, record.kdDif),
            sorter: (a: Player, b: Player) => b.kdDif - a.kdDif
        }, // kd dif
        {
            dataIndex: COLUMN_NAMES.headShot.dataIndex,
            title: COLUMN_NAMES.headShot.readableName,
            className: this.getCellClassName(COLUMN_NAMES.headShot.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, utils.getHeadshotsString(record.headShot, record.kills)),
            sorter: (a: Player, b: Player) => utils.getHeadshotsPercent(b.headShot, b.kills) - utils.getHeadshotsPercent(a.headShot, a.kills)
        }, // headshots
        {
            dataIndex: COLUMN_NAMES.totalGames.dataIndex,
            title: COLUMN_NAMES.totalGames.readableName,
            className: this.getCellClassName(COLUMN_NAMES.totalGames.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, record.totalGames),
            sorter: (a: Player, b: Player) => b.totalGames - a.totalGames
        }, // total games
        {
            dataIndex: COLUMN_NAMES.kills.dataIndex,
            title: COLUMN_NAMES.kills.readableName,
            className: this.getCellClassName(COLUMN_NAMES.kills.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, record.kills),
            sorter: (a: Player, b: Player) => b.kills - a.kills
        }, // kills
        {
            dataIndex: COLUMN_NAMES.assists.dataIndex,
            title: COLUMN_NAMES.assists.dataIndex,
            className: this.getCellClassName(COLUMN_NAMES.assists.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, record.assists),
            sorter: (a: Player, b: Player) => b.assists - a.assists
        }, // assists
        {
            dataIndex: COLUMN_NAMES.deaths.dataIndex,
            title: COLUMN_NAMES.deaths.readableName,
            className: this.getCellClassName(COLUMN_NAMES.deaths.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, record.deaths),
            sorter: (a: Player, b: Player) => b.deaths - a.deaths
        }, // deaths
        {
            dataIndex: COLUMN_NAMES.killsPerGame.dataIndex,
            title: COLUMN_NAMES.killsPerGame.readableName,
            className: this.getCellClassName(COLUMN_NAMES.killsPerGame.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, record.killsPerGame),
            sorter: (a: Player, b: Player) => b.killsPerGame - a.killsPerGame
        }, // kills per game
        {
            dataIndex: COLUMN_NAMES.assistsPerGame.dataIndex,
            title: COLUMN_NAMES.assistsPerGame.readableName,
            className: this.getCellClassName(COLUMN_NAMES.assistsPerGame.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, record.assistsPerGame),
            sorter: (a: Player, b: Player) => b.assistsPerGame - a.assistsPerGame
        }, // assists per game
        {
            dataIndex: COLUMN_NAMES.defusedBombs.dataIndex,
            title: COLUMN_NAMES.defusedBombs.readableName,
            className: this.getCellClassName(COLUMN_NAMES.defusedBombs.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, record.defusedBombs),
            sorter: (a: Player, b: Player) => b.defusedBombs - a.defusedBombs
        }, // defused bombs
        {
            dataIndex: COLUMN_NAMES.explodedBombs.dataIndex,
            title: COLUMN_NAMES.explodedBombs.readableName,
            className: this.getCellClassName(COLUMN_NAMES.explodedBombs.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, record.explodedBombs),
            sorter: (a: Player, b: Player) => b.explodedBombs - a.explodedBombs
        }, // exploded bombs
        {
            dataIndex: COLUMN_NAMES.friendlyKills.dataIndex,
            title: COLUMN_NAMES.friendlyKills.readableName,
            className: this.getCellClassName(COLUMN_NAMES.friendlyKills.dataIndex),
            render: (_link: any, record: Player) => this.cellWrapper(record.id, record.friendlyKills),
            sorter: (a: Player, b: Player) => b.friendlyKills - a.friendlyKills
        }, // friendly kills

    ];

    get columnSelector(): ReactNode {
        const { visibleColumns } = this.state;
        const colsToRender = visibleColumns.filter((x) => !PERMANENT_COLUMNS.includes(x));
        return <ColumnsSelector visibleColumns={colsToRender} onCheckboxesChange={this.onCheckboxesChange} />;
    }

    componentDidMount() {
        const search: any = qs.parse(this.props.router.location.search);
        this.fetchPlayers(this.props.playersDataUrl, search);
    }

    render(): ReactNode {
        const {
            isLoading, players
        } = this.props;

        const search: any = qs.parse(this.props.router.location.search);
        const { dateFrom, dateTo } = search;
        const mappedPlayers = players.map((player) => {
            const cmsPlayer = getPlayerById(player.steamId) || DEFAULT_CMS_PLAYER;
            return ({
                ...player,
                name: cmsPlayer.nickName,
                steamImage: cmsPlayer.steamImage,
            });
        });

        return (
            <>
                <Divider orientation="left">Choose Dates to Filter Statistics</Divider>
                <div className="players-data__filters">
                    <FilterForm
                        onFormSubmit={this.onFormSubmit}
                        isLoading={isLoading}
                        dateFrom={dateFrom}
                        dateTo={dateTo}
                    />
                    <Dropdown overlay={this.columnSelector} trigger={['click']}>
                        <Button className="ant-dropdown-link">
                            Select columns to render <Icon type="down" />
                        </Button>
                    </Dropdown>
                </div>
                <Divider />
                <Table
                    className="players-data"
                    rowClassName={() => 'players-data__row'}
                    columns={this.getColumns()}
                    dataSource={mappedPlayers}
                    rowKey={(record) => record.id}
                    pagination={false}
                    size="middle"
                    bordered={true}
                    scroll={{ x: true }}
                    loading={isLoading}
                    onRow={(record) => ({
                        onClick: () => {
                            this.onRowClick(record);
                        }
                    })}
                />
            </>
        );
    }
}

type PlayersDataProps = {
    playersDataUrl: string;
    isLoading: boolean;
    players: Player[];
    fetchPlayers: typeof fetchPlayers;
    startRequest: typeof startRequest;
    stopRequest: typeof stopRequest;
    router: any;
};

type PlayersDataState = {
    visibleColumns: string[];
};

export type ColumnNames = {
    [key: string]: ColumnMapping;
    imagePath: ColumnMapping;
    name: ColumnMapping;
    points: ColumnMapping;
    kdRatio: ColumnMapping;
    kills: ColumnMapping;
    deaths: ColumnMapping;
    totalGames: ColumnMapping;
    killsPerGame: ColumnMapping;
    headShot: ColumnMapping;
    assists: ColumnMapping;
    assistsPerGame: ColumnMapping;
    defusedBombs: ColumnMapping;
    explodedBombs: ColumnMapping;
    friendlyKills: ColumnMapping;
    kdDif: ColumnMapping;
};

export type ColumnMapping = {
    dataIndex: string;
    readableName: string;
};

const mapStateToProps = (state: RootState) => {
    return ({
        isLoading: state.app.isLoading,
        players: state.app.players,
        router: state.router,
    });
};

export default connect(
    mapStateToProps,
    {
        fetchPlayers,
        startRequest,
        stopRequest
    }
)(PlayersData);
