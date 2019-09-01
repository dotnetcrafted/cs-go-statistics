import React from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
import shortid from 'shortid';
import randomMC from 'random-material-color';
import { withStyles  } from '@material-ui/core/styles';
import Table from '@material-ui/core/Table';
import TableBody from '@material-ui/core/TableBody';
import TableCell from '@material-ui/core/TableCell';
import TableHead from '@material-ui/core/TableHead';
import TableRow from '@material-ui/core/TableRow';
import Paper from '@material-ui/core/Paper';
import Avatar from '@material-ui/core/Avatar';
import PersonIcon from '@material-ui/icons/Person';
import { createMuiTheme } from '@material-ui/core/styles';
import { ThemeProvider } from '@material-ui/styles';
import Container from '@material-ui/core/Container';
import TableBodySkeleton from './table-body-skeleton';

const styles = theme => ({
    root: {
        width: '100%',
        marginTop: theme.spacing(3),
        overflowX: 'auto',
    }
});

class PlayersData extends React.Component {
    constructor(props) {
        super(props);
        this.playersDataUrl = this.props.playersDataUrl;
        this.state = {
            playersData: [],
            isLoading: false
        }
    }
    componentDidMount() {
        this.getPlayersData();
    }
    getPlayersData = () => {
        this.setState({isLoading: true})
        axios.get(this.playersDataUrl).then((response) => {
            this.setState({isLoading: false})
            this.setState({
                playersData: response.data
            });
        }, (error) => {
            this.setState({isLoading: false})
            console.error(error);
        });
    }
    getAvatar(player) {
        const {avatar} = this.props.classes;
        if(player.ImagePath) {
            return <Avatar alt="player.NickName" src={player.ImagePath} />
        } else { 
            return <Avatar><PersonIcon/></Avatar>
        }
    }
    render() {
        const {classes, theme} = this.props;
        const {isLoading} = this.state;
        return (
            <ThemeProvider theme={theme}>
                <Container maxWidth="xl">
                    <Paper className={classes.root}>
                        <Table size="small">
                            <TableHead>
                                <TableRow>
                                    <TableCell></TableCell>
                                    <TableCell>Player Name</TableCell>
                                    <TableCell>Total Games</TableCell>
                                    <TableCell>K/D Ratio</TableCell>
                                    <TableCell>Kills</TableCell>
                                    <TableCell>DeaTableCells</TableCell>
                                    <TableCell>Assists</TableCell>
                                    <TableCell>Head Shot</TableCell>
                                    <TableCell>Defused Bombs</TableCell>
                                    <TableCell>Exploded Bombs</TableCell>
                                    <TableCell>Kills Per Game</TableCell>
                                    <TableCell>Deaths Per Game</TableCell>
                                    <TableCell>Assists Per Game</TableCell>
                                    <TableCell>Favorite Gun</TableCell>
                                </TableRow>
                            </TableHead>
                            {isLoading ? (
                                <TableBodySkeleton/>
                            ) : (
                                <TableBody>
                                    {this.state.playersData.map(item => (
                                        <TableRow key={item.Player.Id || shortid.generate()}>
                                            <TableCell>{this.getAvatar(item.Player)}</TableCell>
                                            <TableCell>{item.Player.NickName}</TableCell>
                                            <TableCell>{item.TotalGames}</TableCell>
                                            <TableCell>{item.KdRatio}</TableCell>
                                            <TableCell>{item.Kills}</TableCell>
                                            <TableCell>{item.Death}</TableCell>
                                            <TableCell>{item.Assists}</TableCell>
                                            <TableCell>{item.HeadShot}</TableCell>
                                            <TableCell>{item.Defuse}</TableCell>
                                            <TableCell>{item.Explode}</TableCell>
                                            <TableCell>{item.KillsPerGame}</TableCell>
                                            <TableCell>{item.DeathPerGame}</TableCell>
                                            <TableCell>{item.AssistsPerGame}</TableCell>
                                            <TableCell>{item.FavoriteGun}</TableCell>
                                        </TableRow>
                                    ))}
                                </TableBody>
                            )}
                        </Table>
                    </Paper>
                </Container>
            </ThemeProvider>
        );
    }
}

PlayersData.propTypes = {
    playersDataUrl: PropTypes.string.isRequired
};
export default withStyles(styles, { withTheme: true })(PlayersData);