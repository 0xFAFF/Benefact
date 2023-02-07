terraform {
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "4.53.0"
    }
  }

  backend "s3" {
    bucket         = "benefact-terraform-state"
    key            = "global/s3/terraform.tfstate"
    region         = "us-east-1"
    dynamodb_table = "benefact-terraform-lock"
    encrypt        = true
  }
}

resource "aws_cloudwatch_log_group" "logs" {
  name = "/ecs/${var.prefix}-logs"
}

resource "aws_ecs_cluster" "main" {
  name = "${var.prefix}-cluster"
}

data "aws_ami" "ami" {
  most_recent = true
  filter {
    name   = "name"
    values = ["amzn2-ami-ecs-hvm-2.0.20230109-x86_64-ebs"]
  }
  # owners = ["099720109477"] # Canonical
}

data "aws_availability_zones" "main" {
  all_availability_zones = true

  filter {
    name   = "region-name"
    values = ["us-east-1"]
  }
}

# resource "aws_network_interface" "main" {
#   subnet_id = aws_subnet.subnet_a.id
#   tags = {
#     Name = "primary_network_interface"
#   }
# }
resource "aws_iam_role" "instance" {

  assume_role_policy = jsonencode({
    Version = "2012-10-17"
    Statement = [
      {
        Action = "sts:AssumeRole"
        Effect = "Allow"
        Sid    = ""
        Principal = {
          Service = "ec2.amazonaws.com"
        }
      },
    ]
  })

  inline_policy {
    name = "${var.prefix}-api-role"
    policy = jsonencode({
      Version = "2012-10-17"
      Statement = [
        {
          Effect = "Allow"
          Action = [
            "elasticfilesystem:DescribeBackupPolicy",
            "elasticfilesystem:DescribeMountTargets",
            "elasticfilesystem:DescribeTags",
            "elasticfilesystem:ListTagsForResource",
            "elasticfilesystem:DescribeLifecycleConfiguration",
            "elasticfilesystem:ClientMount",
            "elasticfilesystem:DescribeFileSystemPolicy",
            "elasticfilesystem:DescribeAccessPoints",
            "elasticfilesystem:DescribeAccountPreferences",
            "elasticfilesystem:ClientWrite",
            "elasticfilesystem:DescribeFileSystems",
            "elasticfilesystem:DescribeMountTargetSecurityGroups"
          ]
          Resource = "*"
        },
      ]
    })
  }
}

resource "aws_iam_instance_profile" "instance" {
  role = aws_iam_role.instance.id
}

resource "aws_instance" "main" {
  tags                        = { "Name" : "${var.prefix}-instance" }
  instance_type               = "t3.micro"
  ami                         = data.aws_ami.ami.id
  associate_public_ip_address = true
  key_name                    = "exurban"
  subnet_id                   = aws_subnet.subnet_a.id
  iam_instance_profile        = aws_iam_instance_profile.instance.id
  security_groups             = [aws_security_group.instance.id]
  # availability_zone = data.aws_availability_zones.main.names[0]
  # network_interface {
  #   network_interface_id = aws_network_interface.main.id
  #   device_index         = 0
  # }
  user_data = <<EOF
#!/bin/bash -xe
echo ECS_CLUSTER=${aws_ecs_cluster.main.name} >> /etc/ecs/ecs.config"
EOF
}
