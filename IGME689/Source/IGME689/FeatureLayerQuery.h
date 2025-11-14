// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "Http.h"
#include "FeatureLayerQuery.generated.h"

USTRUCT(BlueprintType)
struct FGeometries
{
	GENERATED_BODY()

public:
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	TArray<float> Geometry;
};

USTRUCT(Blueprintable)
struct FProperties
{
	GENERATED_BODY()

public:
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Properties")
	FString Name;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Properties")
	int Altitude;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Properties")
	FString Location;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Properties")
	int Length;
	UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Geometry")
	TArray<FGeometries> Geometries;
	
};


UCLASS()
class IGME689_API AFeatureLayerQuery : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AFeatureLayerQuery();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;
	virtual void OnResponseReceived(FHttpRequestPtr Request, FHttpResponsePtr Response, bool bSucceeded);
	virtual void ProcessRequest();
	
	UPROPERTY(EditAnywhere, BlueprintReadWrite)
	TArray<FProperties> features;

private:
	UPROPERTY(EditAnywhere, BlueprintReadWrite, meta = (AllowPrivateAccess = "true"))
	FString webLink = "";
};

