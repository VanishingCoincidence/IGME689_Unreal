// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "APIMapCreator.generated.h"

class UArcGISPoint;
class UArcGISMapComponent;
class UArcGISLocationComponent;

UCLASS()
class IGME689_API AAPIMapCreator : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AAPIMapCreator();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

private:
	UFUNCTION(BlueprintCallable)
	void SetOrigin(UArcGISPoint* NewPosition);
	
	UPROPERTY(BlueprintReadWrite, EditAnywhere, meta = (AllowPrivateAccess = true))
	UArcGISLocationComponent* locationComponent;
	UPROPERTY(BlueprintReadOnly, VisibleAnywhere, meta = (AllowPrivateAccess = true))
	UArcGISMapComponent* mapComponent;

};
