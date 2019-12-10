import React, { Component } from 'react';
import ReactSummernote from 'react-summernote';
import { bindActionCreators } from 'redux';
import { connect } from 'react-redux';
import { actionCreatorsSummernote } from '../store/Summernote';
import PreviewPop from './PreviewPopup';
import "../content/fontawesome/css/font-awesome.min.css";
import 'react-summernote/dist/react-summernote.css';
import $ from 'jquery';
import 'bootstrap/js/modal.js';
import 'bootstrap/js/dropdown.js';
import 'bootstrap/js/tooltip.js';
import 'bootstrap/dist/css/bootstrap.css';

class Summernote extends Component {
    PreviewButton = () => {
        var ui = $.summernote.ui;
        var button = ui.button({
            contents: '<i class="fa fa-eye"/>',
            tooltip: 'Preview',
            click: this.props.PreviewClick
        })
        return button.render();
    }

    onImageUpload(images, insertImage) {    
        for (let i = 0; i < images.length; i++) {          
            const reader = new FileReader();
            reader.onloadend = () => {
                insertImage(reader.result);
            };
            reader.readAsDataURL(images[i]);
        }
    };

    render() {
        return (
            <div>
                <ReactSummernote
                    value={this.props.summernoteContent}
                    options={{
                        height: 300,
                        toolbar: [
                            ['style', ['style']],
                            ['font', ['bold', 'italic', 'underline']],
                            ['para', ['ul', 'ol', 'paragraph']],
                            ['color', ['color']],
                            ['fontname', ['fontname']],
                            ['fontsize', ['fontsize']],
                            ['height', ['height']],
                            ['insert', ['link', 'picture', 'clear', 'hr']],
                            ['view', ['codeview']],
                            ['help', ['help']],
                            ['mybutton', ['preview']]
                        ],
                        disableResizeEditor: true,
                        fontSizes: ['8', '9', '10', '11', '12', '14', '16', '18', '20', '22', '24', '26', '28', '36', '48', '72'],
                        buttons: {
                            preview: this.PreviewButton
                        }
                    }}
                    onChange={this.props.SummernoteChange}
                    onImageUpload={this.onImageUpload}
                />
                <PreviewPop show={this.props.isShow} summernoteContent={this.props.summernoteContent} popupClose={this.props.handleModelHide} />
            </div>
        )
    }
}

function mapStatetoProps(state) {
    return {
        summernoteContent: state.summernote.summernoteContent,
        isShow: state.summernote.isShow
    }
}

export default connect(
    mapStatetoProps,
    dispatch => bindActionCreators(actionCreatorsSummernote, dispatch)
)(Summernote)
