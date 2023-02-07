resource "aws_efs_file_system" "db" {
  performance_mode = "generalPurpose"
  encrypted        = true
  tags             = { "Name" : "${var.prefix}-db-volume" }
}

resource "aws_efs_mount_target" "db" {
  file_system_id = aws_efs_file_system.db.id
  subnet_id      = aws_subnet.subnet_a.id
}

resource "aws_efs_access_point" "db" {
  file_system_id = aws_efs_file_system.db.id
  root_directory {
    creation_info {
      owner_gid   = 999
      owner_uid   = 999
      permissions = "0755"
    }
    path = "/"
  }
}

resource "aws_iam_role" "db_role" {
  name = "${var.prefix}-db-role"

  # Terraform's "jsonencode" function converts a
  # Terraform expression result to valid JSON syntax.
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
    name = "${var.prefix}-db-role"

    policy = jsonencode({
      Version = "2012-10-17"
      Statement = [
        {
          Effect = "Allow"
          Action = [
            "elasticfilesystem:ClientMount",
            "elasticfilesystem:ClientWrite",
          ]
          Resource = "${aws_efs_file_system.db.arn}"
        },
      ]
    })
  }
}

resource "aws_ecs_task_definition" "db" {
  family       = "${var.prefix}-db-task"
  tags         = { "Name" : "${var.prefix}-db-task" }
  network_mode = "bridge"

  container_definitions = jsonencode([
    {
      name        = "db-container"
      image       = "postgres:13.4"
      cpu         = 1
      memory      = 1024
      essential   = true
      mountPoints = [{ "sourceVolume" : "monolith-db-volume", "containerPath" : "/var/lib/postgresql/data" }]
      environment = [{ "name" : "POSTGRES_PASSWORD", "value" : "verysecretpassword" }]
      portMappings = [
        {
          containerPort = 5432
          hostPort      = 5432
        }
      ]
    }
  ])

  task_role_arn = aws_iam_role.db_role.arn

  volume {
    name = "monolith-db-volume"
    efs_volume_configuration {
      file_system_id     = aws_efs_file_system.db.id
      root_directory     = "/"
      transit_encryption = "ENABLED"
      authorization_config {
        access_point_id = aws_efs_access_point.db.id
        iam             = "ENABLED"
      }
    }
  }
  requires_compatibilities = ["EC2"]
}

resource "aws_ecs_service" "db" {
  name = "${var.prefix}-db-service"
  cluster = aws_ecs_cluster.main.id
  task_definition = aws_ecs_task_definition.db.arn
  desired_count = 1
  launch_type = "EC2"
}
