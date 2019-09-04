import React from 'react';
import PropTypes from 'prop-types';
import { RadialChart, Hint, DiscreteColorLegend } from 'react-vis';
import { Typography, Badge } from 'antd';
import randomColor from 'randomcolor';

const { Text  } = Typography;
export default class GunsChart extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            data: [],
            colors: [],
            legendItems: [],
            hoveredChartSection: false
        };
    }

    componentWillMount() {
        const {guns} = this.props;
        this._updateState(guns);
    }
    componentWillReceiveProps(nextProps) {
        const {guns} = nextProps;
        this._updateState(guns);
    }
    
    render() {
        const { data, legendItems, hoveredChartSection } = this.state;
        return (
            <div className="guns-chart__container">
                <DiscreteColorLegend
                    className="guns-chart__legend"
                    items={legendItems}
                />
                <RadialChart
                    className={'donut-chart-example'}
                    innerRadius={50}
                    radius={100}
                    getAngle={(d) => d.theta}
                    data={data}
                    onValueMouseOver={(v) => this.setState({ hoveredChartSection: v })}
                    onSeriesMouseOut={(v) => this.setState({ hoveredChartSection: false })}
                    width={200}
                    height={200}
                    padAngle={0.04}
                    animation={{damping: 9, stiffness: 300}}
                >
                    {hoveredChartSection !== false &&
                        <Hint value={hoveredChartSection} >
                            <Badge count={hoveredChartSection.theta}>
                                <div className='guns-chart__text'>{hoveredChartSection.label}</div>
                            </Badge>
                        </Hint>
                    }
                </RadialChart>
            </div>
        );
    }

    _updateState(guns) {
        const colors = this._getColors(guns);
        const data = this._getData(guns, colors);
        const legendItems = this._getLegend(guns, colors);
        this.setState({colors, data, legendItems});
    }
    _getData = (guns, colors) => ( guns.map((g) => ({
        theta: g.Kills,
        label: g.Name,
        style: {
            fill: colors.find(c => c.id === g.Id).color,
            stroke:false
        }
    })))
    _getColors = (guns) => ( guns.map((g) => ({
        id: g.Id,
        color: randomColor({
            luminosity: 'bright',
            hue: 'random'
        })
    })))
    _getLegend = (guns, colors) => ( guns.map((g) => ({
        title: `${g.Name}: ${g.Kills} kills`,
        color: colors.find(c => c.id === g.Id).color
    })))
}
GunsChart.propTypes = {
    guns: PropTypes.array.isRequired,
};
