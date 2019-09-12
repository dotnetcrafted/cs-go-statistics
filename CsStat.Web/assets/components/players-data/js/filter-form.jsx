import React from 'react';
import PropTypes from 'prop-types';
import {
    Form, DatePicker, Button
} from 'antd';
import { connect } from 'react-redux';
import moment from 'moment';

const { RangePicker } = DatePicker;
const DATE_FORMAT = 'MM/DD/YYYY';
class FilterForm extends React.Component {
    _handleSubmit = (e) => {
        e.preventDefault();
        this.props.form.validateFields((err, fieldsValue) => {
            if (err) {
                return;
            }
    
            const rangeValue = fieldsValue['range-time-picker'];
            const values = {
                dateFrom: rangeValue[0].format(DATE_FORMAT),
                dateTo: rangeValue[1].format(DATE_FORMAT)
            };

            this.props.onFormSubmit(values);
        });
    };
    render() {
        const {dateFrom, dateTo} = this.props;
        const { getFieldDecorator} = this.props.form;
        const rangeConfig = {
            rules: [{ 
                type: 'array',
                required: true,
                message: 'Please select date.'
            }],
            initialValue:[moment(dateFrom, 'DD.MM.YYYY'), moment(dateTo, 'DD.MM.YYYY')]
        };
        
        return (
            <Form layout="inline" onSubmit={this._handleSubmit}>
                <Form.Item>
                    {getFieldDecorator('range-time-picker', rangeConfig)(
                        <RangePicker format={DATE_FORMAT} />,
                    )}
                </Form.Item>
                <Form.Item>
                    <Button 
                        type="primary"
                        htmlType="submit"
                        disabled={this.props.isLoading}
                    >Filter</Button>
                </Form.Item>
            </Form>
        )
    }
};



FilterForm.propTypes = {
    onFormSubmit: PropTypes.func.isRequired,
    isLoading: PropTypes.bool.isRequired,
    dateFrom: PropTypes.string.isRequired,
    dateTo: PropTypes.string.isRequired
};


const WrappedFilterForm = Form.create({ name: 'filter_form' })(FilterForm);

export default WrappedFilterForm;