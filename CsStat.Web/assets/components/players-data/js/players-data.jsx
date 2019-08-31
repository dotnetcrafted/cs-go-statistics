import React from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
import shortid from 'shortid';
import randomMC from 'random-material-color';
import { makeStyles } from '@material-ui/core/styles';
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

export default class PlayersData extends React.Component {

    constructor(props) {
        super(props);

        this.playersDataUrl = this.props.playersDataUrl;
        this.classes = makeStyles({
            avatar: {
                backgroundColor: randomMC.getColor()
            }
        });

        this.theme = createMuiTheme({
            palette: {
              type: 'dark',
            },
          });

        this.state = {
            playersData: []
        }
    }
    componentDidMount() {        
        this.getPlayersData();
    }
    getPlayersData = () => {
        axios.get(this.playersDataUrl).then((response) => {
            this.setState({
                playersData: response.data
            });
        }, (error) => {
            console.error(error);
        });
    }
    getAvatar(player) {
        if(player.ImagePath) {
            return <Avatar alt="player.NickName" src={player.ImagePath} />
        } else { 
            return <Avatar className={this.classes.avatar}><PersonIcon/></Avatar>
        }
    }
    render() {
        return (
            <ThemeProvider theme={this.theme}>
                <Container maxWidth="xl">
                    <Paper>
                        <Table>
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
