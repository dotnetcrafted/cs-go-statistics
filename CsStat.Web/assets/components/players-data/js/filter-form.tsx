import React from 'react';
import {Form, DatePicker, Button} from 'antd';
import { FormComponentProps } from 'antd/es/form';
import moment from 'moment';

const { RangePicker } = DatePicker;
const SERVER_DATE_FORMAT = 'MM/DD/YYYY';
const USER_DATE_FORMAT = 'll';

class FilterForm extends React.Component<IFilterFormProps, any> {
    _handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        this.props.form.validateFields((err: any, fieldsValue) => {
            if (err) {
                return;
            }
    
            const rangeValue = fieldsValue['range-time-picker'];
            const values: IDateValues = {
                dateFrom: rangeValue[0].format(SERVER_DATE_FORMAT),
                dateTo: rangeValue[1].format(SERVER_DATE_FORMAT)
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
            initialValue:[moment(dateFrom, SERVER_DATE_FORMAT), moment(dateTo, SERVER_DATE_FORMAT)]
        };
        
        return (
            <Form layout="inline" onSubmit={this._handleSubmit}>
                <Form.Item>
                    {getFieldDecorator('range-time-picker', rangeConfig)(
                        <RangePicker format={USER_DATE_FORMAT}/>,
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

export interface IDateValues {
    [index: string] : string
    dateFrom: string
    dateTo: string
}
interface IFilterFormProps extends FormComponentProps {
    onFormSubmit: (message: IDateValues) => void;
    dateFrom: string
    dateTo: string
    isLoading: boolean
}
const WrappedFilterForm = Form.create<IFilterFormProps>({ name: 'filter_form' })(FilterForm);

export default WrappedFilterForm;
