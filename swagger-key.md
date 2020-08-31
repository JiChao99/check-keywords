# swagger

- `swagger` swagger版本
- `info` API信息
    - `version` API版本
    - `title` API标题
- `host` API domain
- `basePath` API base url
- `tags`
- `paths` API address array
    - `{API address}`
    - `tags`
    - `summary` 概要
    - `parameters`
        - 📌`name`
- `definitions`
    - `{className}`
        - `type`
        - `properties`
        - 📌`{paramName}`
            - `type`
            - `description`
- `securityDefinitions`
    - `{authtype}`

```json
{
    "swagger": "2.0",
    "info": {
        "version": "v1",
        "title": "网络问政 移动办公 API"
    },
    "host": "192.168.1.32:8030",
    "schemes": [
        "http"
    ],
    "paths": {
        "/wz/v1/baseInfos": {
            "get": {
                "tags": [
                    "QuestionHandlingAPI"
                ],
                "summary": "获取平台信息",
                "operationId": "QuestionHandlingAPI_GetBsaeInfo",
                "consumes": [],
                "produces": [
                    "application/json",
                    "text/json",
                    "application/xml",
                    "text/xml"
                ],
                "parameters": [
                    {
                        "name": "newspaperGroupId",
                        "in": "query",
                        "description": "报社组Id",
                        "required": true,
                        "type": "integer",
                        "format": "int32"
                    }
                ],
                "responses": {
                    "200": {
                        "description": "获取信息成功",
                        "schema": {
                            "$ref": "#/definitions/NewspaperGroupBaseInfo"
                        }
                    },
                    "404": {
                        "description": "找不到当前平台信息"
                    }
                }
            }
        },
        "/wz/v1/questions": {
            "get": {
                "tags": [
                    "QuestionHandlingAPI"
                ],
                "summary": "获取问题列表",
                "operationId": "QuestionHandlingAPI_GetQuestions",
                "consumes": [],
                "produces": [
                    "application/json",
                    "text/json",
                    "application/xml",
                    "text/xml"
                ],
                "parameters": [
                    {
                        "name": "Authorization",
                        "in": "header",
                        "description": "token",
                        "required": false,
                        "type": "string"
                    },
                    {
                        "name": "questionStatus",
                        "in": "query",
                        "description": "问题状态[0-待分配，1-办理中，2-已办理]",
                        "required": false,
                        "type": "integer",
                        "format": "int32"
                    },
                    {
                        "name": "type",
                        "in": "query",
                        "description": "问题类型[1-咨询，2-感谢，3-投诉，4-建议，5-求助]",
                        "required": false,
                        "type": "integer",
                        "format": "int32"
                    },
                    {
                        "name": "departId",
                        "in": "query",
                        "description": "部门Id",
                        "required": false,
                        "type": "integer",
                        "format": "int32"
                    },
                    {
                        "name": "pageIndex",
                        "in": "query",
                        "description": "当前页码",
                        "required": false,
                        "type": "integer",
                        "format": "int32"
                    },
                    {
                        "name": "pagesize",
                        "in": "query",
                        "description": "分页大小",
                        "required": false,
                        "type": "integer",
                        "format": "int32"
                    }
                ],
                "responses": {
                    "200": {
                        "description": "获取问题列表成功",
                        "schema": {
                            "$ref": "#/definitions/QuestionsResponse"
                        }
                    }
                }
            }
        }
    },
    "definitions": {
        "NewspaperGroupBaseInfo": {
            "description": "基本信息",
            "type": "object",
            "properties": {
                "logo": {
                    "description": "LOG",
                    "type": "string"
                },
                "title": {
                    "description": "名称",
                    "type": "string"
                },
                "description": {
                    "description": "简介",
                    "type": "string"
                },
                "questionCount": {
                    "format": "int32",
                    "description": "问题总数",
                    "type": "integer"
                }
            }
        },
        "QuestionsResponse": {
            "description": "问题列表",
            "type": "object",
            "properties": {
                "questionList": {
                    "description": "问题列表",
                    "type": "array",
                    "items": {
                        "$ref": "#/definitions/QuestionResponse"
                    }
                },
                "sumCount": {
                    "format": "int32",
                    "description": "总问题数量",
                    "type": "integer"
                }
            }
        }
    },
    "securityDefinitions": {
        "Bearer": {
            "type": "basic",
            "description": "Basic HTTP Authentication"
        }
    }
}
```
