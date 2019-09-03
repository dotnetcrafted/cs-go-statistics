import React from 'react';
import PropTypes from 'prop-types';
import { RadialChart, Hint, DiscreteColorLegend } from 'react-vis';
import { Typography, Badge } from 'antd';
const { Text  } = Typography;
export default class GunsChart extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            value: false
        };
    }

    render() {
        const { value } = this.state;
        const {guns} = this.props;
        return (
            <div className="guns-chart__container">
                <RadialChart
                    className={'donut-chart-example'}
                    innerRadius={50}
                    radius={100}
                    getAngle={(d) => d.theta}
                    data={this.getData(guns)}
                    onValueMouseOver={(v) => this.setState({ value: v })}
                    onSeriesMouseOut={(v) => this.setState({ value: false })}
                    width={200}
                    height={200}
                    padAngle={0.04}
                >
                    {value !== false &&
                        <Hint value={value} >
                            <Badge count={value.theta}>
                                <div className='guns-chart__text'>{value.label}</div>
                            </Badge>
                        </Hint>
                    }
                </RadialChart>
                {/* <DiscreteColorLegend height={200} width={300} items={this.getLegend(guns)} /> */}
            </div>
        );
    }
    getData = (guns) => ( guns.map((g) => ({theta: g.Kills, label: g.Name})) )
    getLegend = (guns) => ( guns.map((g) => g.Name) )
}
GunsChart.propTypes = {
    guns: PropTypes.array.isRequired,
};
