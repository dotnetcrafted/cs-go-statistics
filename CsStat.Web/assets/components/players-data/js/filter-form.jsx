import React from 'react';
import PropTypes from 'prop-types';
import {
    Form, DatePicker, Button
} from 'antd';
import { connect } from 'react-redux';

const { RangePicker } = DatePicker;

class FilterForm extends React.Component {
    _handleSubmit = (e) => {
        e.preventDefault();
        this.props.form.validateFields((err, fieldsValue) => {
            if (err) {
                return;
            }
    
            const rangeValue = fieldsValue['range-time-picker'];
            const values = {
                dateFrom: rangeValue[0].format('MM/DD/YYYY'),
                dateTo: rangeValue[1].format('MM/DD/YYYY')
            };

            this.props.onFormSubmit(values);
        });
    };
    render() {
        const { getFieldDecorator} = this.props.form;
        const rangeConfig = {
            rules: [{ type: 'array', required: true, message: 'Please select date.' }],
        };
        return (
            <Form layout="inline" onSubmit={this._handleSubmit}>
                <Form.Item>
                    {getFieldDecorator('range-time-picker', rangeConfig)(
                        <RangePicker format="YYYY-MM-DD" />,
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
    isLoading: PropTypes.bool.isRequired
};


const WrappedFilterForm = Form.create({ name: 'filter_form' })(FilterForm);

export default WrappedFilterForm;